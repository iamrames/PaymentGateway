namespace PaymentGateway.Domain.Options;
public class RabbitMqOptions
{
    public required Uri HostAddress { get; set; }

    public required string VirtualHost { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }
    public bool UseSSL { get; set; } = false;

    private string? _connectionString;

    public string ConnectionString
    {
        get
        {
            if (_connectionString != null)
            {
                return _connectionString;
            }

            _connectionString = new UriBuilder(HostAddress)
            {
                UserName = Username,
                Password = Password,
                Path = "/%2F"
            }
                .Uri
                .ToString();

            return _connectionString;
        }
    }

    /// <inheritdoc />
    public override string ToString() => $"[HostAddress:{HostAddress}][VirtualHost:{VirtualHost}][Username:{Username}][Password:{Password}]";
}
