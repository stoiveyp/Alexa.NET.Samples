
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunction
{
    public static class Function1
    {
        [FunctionName("AlexaNetSample")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]SkillRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            if (req.Request is LaunchRequest)
            {
                return new OkObjectResult(ResponseBuilder.Tell("you launched from method param"));
            }

            return new OkObjectResult(ResponseBuilder.Empty());
        }

    }
}
