using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MessageQueue;

public static class MQExtensions
{
    public static IServiceCollection AddMQPublisher(this IServiceCollection services, bool isInContainer)
    {
        services.AddMassTransit(_ =>
        {
            Action<IRabbitMqHostConfigurator> hostConfig = _ =>
            {
                _.Username(MQConstants.Vhost);
                _.Password(MQConstants.Password);

                _.UseSsl(s =>
                {
                    s.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                });
            };

            _.UsingRabbitMq((context, config) =>
            {
                try
                {
                    config.Host(MQConstants.Host, 5671, MQConstants.Vhost, hostConfig);
                }
                catch (Exception ex)
                {
                    config.Host(MQConstants.FallbackHost, 5671, MQConstants.Vhost, hostConfig);
                }
            });
        });

        return services;
    }
}