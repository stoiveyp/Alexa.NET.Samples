using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ProgressiveResponse
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
            var progressive = new Alexa.NET.Response.ProgressiveResponse(input);
            await Task.Delay(TimeSpan.FromSeconds(2));

            if (progressive.CanSend())
            {
                await progressive.SendSpeech("processing your request. Please wait.");
            }

            await Task.Delay(TimeSpan.FromSeconds(2));

            if (progressive.CanSend())
            {
                await progressive.SendSpeech("Any second now.");
            }

            return ResponseBuilder.Tell("This is the real response");
        }
    }
}
