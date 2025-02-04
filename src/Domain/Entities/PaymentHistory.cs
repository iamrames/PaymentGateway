using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Entities;
public class PaymentHistory
{
    public PaymentHistory(){   }
    public PaymentHistory(PaymentStates? state,
        decimal? amount,
        string? currency,
        string? bankId,
        string? rejectionReason)
    {
        State = state;
        Amount = amount;
        Currency = currency;
        BankId = bankId;
        RejectionReason = rejectionReason;
    }
    public PaymentStates? State { get; private set; }
    public Guid Id { get; private set; }
    public decimal? Amount { get; private set; }
    public string? Currency { get; private set; }
    public Guid PaymentId { get; set; }
    public string? BankId { get; private set; }
    public string? RejectionReason { get; private set; }

    public static PaymentHistory Create(PaymentStates? state,
        decimal? amount, 
        string? currency,
        string? bankId,
        string? rejectionReason)
    {
        var history = new PaymentHistory(state, amount, currency, bankId, rejectionReason);
        return history;
    }
}
