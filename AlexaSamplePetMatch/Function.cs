using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaSamplePetMatch
{
    public class Function
    {

        private RequestManager RequestManager { get; }

        public Function()
        {
            RequestManager = new RequestManager();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            return RequestManager.Process(input);
        }
    }
}
