using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.UnitTests;

public class PaymentTests
{
    [Fact]
    public void Create_WithValidData_Success()
    {
        var payment = Payment.Create((decimal)123.56, "EUR",
            new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                12, DateTime.Today.Year, 765),
            new Customer("Tyrion Lannister", "scarred_dwarf@got.com"));

        Assert.Single(payment.History);
        Assert.IsType<Payment>(payment);
    }

    [Fact]
    public void Create_WithAmountZero_FailsWithException()
    {
        Assert.Throws<ArgumentException>(() => Payment.Create(0, "EUR",
            new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                12, DateTime.Today.Year, 765),
            new Customer("Tyrion Lannister", "scarred_dwarf@got.com"))
        );
    }

    [Fact]
    public void Create_WithoutCurrency_FailsWithException()
    {
        Assert.Throws<ArgumentNullException>(() => Payment.Create(2, null,
            new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                12, DateTime.Today.Year, 765),
            new Customer("Tyrion Lannister", "scarred_dwarf@got.com"))
        );
    }

    [Fact]
    public void Create_WithoutCard_FailsWithException()
    {
        Assert.Throws<ArgumentNullException>(() => Payment.Create(2, null,
            null, null)
        );
    }

    [Fact]
    public void Create_WithExpiredCard_FailsWithException()
    {
        var expiredDate = DateTime.Today.AddMonths(-1);

        Assert.Throws<ArgumentException>(() =>
            Payment.Create((decimal)123.56, "EUR",
                new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    expiredDate.Month, expiredDate.Year, 765),
                new Customer("Tyrion Lannister", "scarred_dwarf@got.com"))
        );
    }

    [Fact]
    public void Processed_WithBankId_StateChanged()
    {
        var payment = Payment.Create((decimal)123.56, "EUR",
            new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                DateTime.Today.Month, DateTime.Today.Year, 765),
            new Customer("Tyrion Lannister", "scarred_dwarf@got.com"));

        payment.Processed("124567");

        Assert.Equal(PaymentStates.Success, payment.State);
    }

    [Fact]
    public void Processed_WithoutBankId_FailsWithException()
    {
        var payment = Payment.Create((decimal)123.56, "EUR",
            new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                DateTime.Today.Month, DateTime.Today.Year, 765),
            new Customer("Tyrion Lannister", "scarred_dwarf@got.com"));

        Assert.Throws<ArgumentNullException>(() => payment.Processed(""));
    }

    [Fact]
    public void Rejected_StateChanged()
    {
        var payment = Payment.Create((decimal)123.56, "EUR",
            new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                DateTime.Today.Month, DateTime.Today.Year, 765),
            new Customer("Tyrion Lannister", "scarred_dwarf@got.com"));

        payment.Rejected("EXPIRED");

        Assert.Equal(PaymentStates.Failed, payment.State);
    }

}
