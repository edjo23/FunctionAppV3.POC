using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionApp
{
    public static class StaticFunction
    {
        [FunctionName("StaticFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Config] IConfiguration configuration,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = $"{configuration.GetValue<string>("GREETING")} from {configuration.GetValue<string>("FUNCTIONS_WORKER_RUNTIME")} {nameof(StaticFunction)}";

            return new OkObjectResult(responseMessage);
        }
    }
}
