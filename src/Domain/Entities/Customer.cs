namespace PaymentGateway.Domain.Entities;
public class Customer
{
    public Customer(){}
    public Customer(string name, string email)
    {
        Name = name; 
        Email = email;
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public ICollection<Payment>? Payments { get; set; }
}
