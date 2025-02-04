namespace PaymentGateway.Web.ServiceBus;

public static class ServiceBusQueues
{
    public static class Payment
    {
        public const string PaymentInitiate = "payment.initiate";
    }
}
