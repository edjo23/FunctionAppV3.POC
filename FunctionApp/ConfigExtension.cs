using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using System;

namespace FunctionApp
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    [Binding]
    public sealed class ConfigAttribute : Attribute
    {
    }

    internal class ConfigProvider : IExtensionConfigProvider
    {
        public ConfigProvider(IConfiguration config)
        {
            _configuration = config;
        }

        private readonly IConfiguration _configuration;

        public void Initialize(ExtensionConfigContext context) => context.AddBindingRule<ConfigAttribute>().BindToInput(_ => _configuration);
    }
}
