using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.ListManagement;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ListManagement
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var handler = new LoggingMessageHandler { InnerHandler = new HttpClientHandler() };

            try
            {
                var token = input.Context.System.ApiAccessToken;
                var http = new HttpClient(handler)
                {
                    BaseAddress = new Uri(ListManagementClient.ListManagementDomain, UriKind.Absolute)
                };
                var client = new Alexa.NET.ListManagementClient(token, http);

                var metadata = await client.GetListsMetadata();
                var firstList = metadata.First();

                handler.CurrentLogs.AppendLine("got first list");
                var item = await client.AddItem(firstList.ListId, "test", SkillListItemStatus.Active);
                Console.WriteLine(JObject.FromObject(item).ToString(Formatting.None));

                return ResponseBuilder.Tell("test");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error found");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("logs:");
                Console.WriteLine(handler.CurrentLogs.ToString());
                return ResponseBuilder.Tell("error");
            }
        }
    }
}
