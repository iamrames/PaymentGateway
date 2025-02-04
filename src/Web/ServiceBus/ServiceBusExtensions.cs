using MassTransit;
using PaymentGateway.Domain.Options;
using System.Security.Authentication;

namespace PaymentGateway.Web.ServiceBus;

public static class ServiceBusExtensions
{
    public static void AddServiceBus(this IServiceCollection services, IConfiguration configuration,
        ServiceBusConfig serviceBusConfig)
    {
        var rabbitMqOptions = configuration.GetSection("ServiceBus:RabbitMQ").Get<RabbitMqOptions>();
        services.AddMassTransit(mt =>
        {
            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions!.HostAddress, rabbitMqOptions.VirtualHost, h =>
                {
                    h.Username(rabbitMqOptions.Username);
                    h.Password(rabbitMqOptions.Password);
                    if (rabbitMqOptions.UseSSL)
                    {
                        h.UseSsl(s => s.Protocol = SslProtocols.Tls12);
                    }
                });

                cfg.PublishTopology.BrokerTopologyOptions = PublishBrokerTopologyOptions.MaintainHierarchy;


                foreach (var messageConfiguration in serviceBusConfig.EventHandlers)
                {
                    cfg.ReceiveEndpoint(messageConfiguration.QueueName!, e =>
                    {
                        if (messageConfiguration.HandlerType != null)
                        {
                            mt.AddConsumer(messageConfiguration.HandlerType);
                        }
                    });
                }
            });
        });
    }

    private static void MapEndpointConvention<T>(Uri uri)
            where T : class
    {
        EndpointConvention.Map<T>(uri);
    }
}
