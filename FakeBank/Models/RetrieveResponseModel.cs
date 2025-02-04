using FakeBank.Enums;

namespace FakeBank.Models;

public class RetrieveResponseModel
{
    public Guid Id { get; set; }
    public PaymentStates State { get; set; }
}
