using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Domain.Constants;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.BankProvider;
using PaymentGateway.Infrastructure.Data;
using PaymentGateway.Infrastructure.Identity;
using PaymentGateway.Infrastructure.Repositories;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddHttpClient("FakeBank", c =>
        {
            c.BaseAddress = new Uri(configuration.GetValue<string>("FakeBankService")!);
        });

        services.AddMediatR(cfg =>  cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
      
        services.AddScoped<IAcquiringBank, FakeBankClient>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlite(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
        services.AddScoped<IUnitOfWork, EntityFrameworkUnitOfWork>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}
