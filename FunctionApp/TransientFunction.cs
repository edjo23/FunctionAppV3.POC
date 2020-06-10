using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionApp
{
    public class TransientFunction
    {
        public TransientFunction(IConfiguration config)
        {
            _configuration = config;
        }

        public readonly IConfiguration _configuration;

        [FunctionName("TransientFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = $"{_configuration.GetValue<string>("GREETING")} from {_configuration.GetValue<string>("FUNCTIONS_WORKER_RUNTIME")} {nameof(TransientFunction)}";

            return new OkObjectResult(responseMessage);
        }
    }
}
