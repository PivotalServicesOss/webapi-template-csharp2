using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

internal static class TestApplicationFactoryExtensions
{
    public static WebApplicationFactory<TEntryPoint> WithTestConfiguration<TEntryPoint>(this WebApplicationFactory<TEntryPoint> factory,
                                                                                ITestOutputHelper outputHelper)
                                                                                where TEntryPoint : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
                /// ... Override or add any settings here
                builder.UseSetting("AnyOverrideSetting", "value");

                /// ... Override app configurations
                builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configBuilder.AddJsonFile("appsettings.Integration.json", optional: false, reloadOnChange: false);
            });
            builder.ConfigureServices(services =>
            {
                    /// ... Configure services here
                });
            builder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddXUnit(outputHelper)
                                .AddConsole()
                                .AddDebug();
            });
        });
    }
}