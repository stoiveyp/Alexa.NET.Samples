using System;
using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace AlexaSamplePetMatch.RequestHandlers
{
    public class LaunchRequestHandler:SynchronousRequestHandler
    {
        private const string response = "Welcome to pet match. I can help you find the best dog for you. What are two things you are looking for in a dog?";
        private const string reprompt = "What size and temperament are you looking for in a dog?";

        public override bool CanHandle(RequestInformation information)
        {
            return information.SkillRequest.Request is LaunchRequest;
        }

        public override SkillResponse HandleSyncRequest(RequestInformation information)
        {
            return ResponseBuilder.Ask(response, new Reprompt(reprompt));
        }
    }
}
