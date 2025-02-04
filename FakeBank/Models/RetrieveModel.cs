namespace FakeBank.Models;

public class RetrieveModel
{
    public decimal Amount { get; set; }
    public required string CardName { get; set; }
    public required string CardNumber { get; set; }
    public int CardExpireMonth { get; set; }
    public int CardExpireYear { get; set; }
    public int CardCVV { get; set; }
}
