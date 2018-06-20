using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace AlexaSamplePetMatch
{
    public class ErrorHandler:AlwaysTrueErrorHandler
    {
        public override async Task<SkillResponse> Handle(RequestInformation information, Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return ResponseBuilder.Tell("So sorry, but something appears to have gone wrong");
        }
    }
}
