using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Infrastructure.Data;
using PaymentGateway.Web.ServiceBus;
using PaymentGateway.Web.Services;


namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceBus(configuration, MessageBroker.ServiceBusConfig);

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddRazorPages();
        services.AddControllers();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "CleanArchitecture API";


            // Add JWT
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        return services;
    }
}
