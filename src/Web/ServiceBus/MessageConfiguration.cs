namespace PaymentGateway.Web.ServiceBus;

public class ServiceBusConfig
{
    public List<MessageConfiguration> EventHandlers { get; set; } = new List<MessageConfiguration>();
    public List<MessageConfiguration> CommandEndpoints { get; set; } = new List<MessageConfiguration>();
}

public class MessageConfiguration
{
    public Type? MessageType { get; set; }

    public Type? HandlerType { get; set; }

    public string? QueueName { get; set; }
    public string? ExchangeType { get; set; }

    /// <inheritdoc />
    public override string ToString() => $"[MessageType:{MessageType!.Name}][HandlerType:{HandlerType!.Name}][QueueName:{QueueName}]";
}
