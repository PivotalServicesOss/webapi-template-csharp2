using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;
using PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;
using HealthChecks.UI.Client;
using Steeltoe.Extensions.Configuration.Kubernetes;
using Steeltoe.Extensions.Configuration.Placeholder;
using Steeltoe.Common;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

[ExcludeFromCodeCoverage]
public partial class Program
{
    public static void Main(string[] args)
    {
        CreateBootstrapLogger();

        try
        {
            Log.Information("Application is starting");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            //Configure Application Configuration
            ConfigureAppConfiguration(args, builder);

            //Configure Logging
            ConfigureAppConfigurationLogging(builder);

            // Add services to the container.
            builder.Services.AddMvcPipeline(builder.Configuration, typeof(Program).Assembly);
            builder.Services.AddOpenApiSwaggerDocumentation(builder.Configuration);

            //Add all health checks here
            builder.Services.AddHealthChecks()
                .AddDiskStorageHealthCheck(options => { options.CheckAllDrives = true; });

            //Add autofac registrations here
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterAssemblyModules(AppDomain.CurrentDomain.GetAssemblies());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.AddGlobalMiddlewares();
            app.UseOpenApiSwaggerDocumentation();
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMvcPipeline();
            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Application failed to start");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        static void ConfigureAppConfiguration(string[] args, WebApplicationBuilder builder)
        {
            builder.Configuration.AddUserSecrets<Program>(optional: true);
            builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false);

            if (Platform.IsKubernetes)
                builder.Configuration.AddKubernetes();

            if (Platform.IsCloudFoundry)
                builder.Configuration.AddCloudFoundry();

            builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddCommandLine(args);
            ((IConfigurationBuilder)builder.Configuration).AddPlaceholderResolver();
        }

        static void ConfigureAppConfigurationLogging(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((context, loggingConfiguration) => 
            {
                loggingConfiguration.ReadFrom.Configuration(context.Configuration);
            });
        }

        static void CreateBootstrapLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateBootstrapLogger();
        }
    }
}


