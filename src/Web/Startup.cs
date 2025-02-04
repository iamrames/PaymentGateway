using PaymentGateway.Infrastructure.Data;


namespace PaymentGateway.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(Configuration);
        services.AddWebServices(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.InitialiseDatabaseAsync().GetAwaiter().GetResult();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseHealthChecks("/health");
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwaggerUi(settings =>
        {
            settings.Path = "/api";
            settings.DocumentPath = "/api/specification.json";
        });

        app.UseExceptionHandler(options => { });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapDefaultControllerRoute();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}
