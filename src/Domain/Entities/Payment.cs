using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Entities;
public class Payment : Entity
{

#pragma warning disable IDE0044 // Add readonly modifier
    private List<PaymentHistory>? _history;
#pragma warning restore IDE0044 // Add readonly modifier

    private Payment() { }

    public Payment(decimal amount,
        string? currency,
        PaymentCard? paymentCard,
        Customer? customer,
        List<PaymentHistory> history)
    {
        Amount = amount;
        Currency = currency;
        Card = paymentCard;
        Customer = customer;
        _history = history;
    }

    public static Payment Create(decimal amount, 
        string? currency, 
        PaymentCard? paymentCard, 
        Customer? customer)
    {
        if (amount <= 0) throw new ArgumentException("Payment amount can't be zero or negative.", nameof(amount));
        if (string.IsNullOrEmpty(currency)) throw new ArgumentNullException(nameof(currency));
        if (paymentCard == null) throw new ArgumentNullException(nameof(paymentCard));
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        if (DateTime.Today > new DateTime(paymentCard.ExpireYear, paymentCard.ExpireMonth, DateTime.Today.Day))
            throw new ArgumentException("Payment card expired.");

        var payment = new Payment(amount, currency, paymentCard, customer,
            history: [
                PaymentHistory.Create(PaymentStates.Pending, amount, currency, null, null)
                ]);
        return payment;
    }

    public PaymentStates? State { get; private set; }
    public Guid Id { get; private set; }

    public decimal? Amount { get; private set; }

    public string? Currency { get; private set; }

    public string? CardNumber { get; private set; }
    public PaymentCard? Card { get; private set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; private set; }

    public string? BankId { get; private set; }
    public string? RejectionReason { get; private set; }

    public IReadOnlyCollection<PaymentHistory> History => _history!.AsReadOnly();

    public void Processed(string bankId)
    {
        if (string.IsNullOrEmpty(bankId)) throw new ArgumentNullException(nameof(bankId));

        BankId = bankId;
        State = PaymentStates.Success;
        _history!.Add(PaymentHistory.Create(State, Amount, Currency, bankId, RejectionReason));
    }

    public void Rejected(string reason)
    {
        State = PaymentStates.Failed;
        RejectionReason = reason;
        _history!.Add(PaymentHistory.Create(State, Amount, Currency, BankId, reason));
    }
}
