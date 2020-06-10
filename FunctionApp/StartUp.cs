using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: FunctionsStartup(typeof(FunctionApp.StartUp))]
namespace FunctionApp
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configFactory = builder.Services.FirstOrDefault(o => o.ServiceType == typeof(IConfiguration));

            builder.Services.Replace(ServiceDescriptor.Singleton<IConfiguration>((sp) =>
            {
                var config = new ConfigurationBuilder()
                    .AddConfiguration(configFactory.ImplementationFactory.Invoke(sp) as IConfiguration)
                    .AddInMemoryCollection(new Dictionary<string, string> { { "GREETING", "Hello POC" } })
                    .Build();
                return config;
            }));

            var webJobBuilder = builder.Services.AddWebJobs(_ => { });
            webJobBuilder.AddExtension<BeefExtension>();
            webJobBuilder.AddExtension<ConfigProvider>();
        }
    }

    public class BeefExtension : IExtensionConfigProvider
    {
        public BeefExtension(ILoggerFactory loggerFactory )
        {
            _logger = loggerFactory.CreateLogger(nameof(BeefExtension));
        }

        public readonly ILogger _logger;

        public void Initialize(ExtensionConfigContext context)
        {
            BeefMock.Register();
            _logger.LogWarning("Beef initialised");
        }
    }

    public static class BeefMock
    {
        private static bool _registered = false;

        public static void Register()
        {
            if (_registered)
                throw new Exception("Cannot register twice");
            _registered = true;
        }
    }
}
