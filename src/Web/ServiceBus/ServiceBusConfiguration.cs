using PaymentGateway.Domain.Contracts;

namespace PaymentGateway.Web.ServiceBus;

public static class MessageBroker
{

    public static ServiceBusConfig ServiceBusConfig = new ServiceBusConfig()
    {
        EventHandlers = new List<MessageConfiguration>
        { 
            new MessageConfiguration
            {
                MessageType = typeof(PaymentInitiateMessage),
                HandlerType = typeof(PaymentInitiateConsumer),
                QueueName = "payment-initiate"
            }
        }
    };
}
