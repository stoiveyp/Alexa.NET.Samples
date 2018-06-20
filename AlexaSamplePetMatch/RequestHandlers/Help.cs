using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace AlexaSamplePetMatch.RequestHandlers
{
    public class Help:SynchronousRequestHandler
    {
        public override bool CanHandle(RequestInformation information)
        {
            if (information.SkillRequest.Request is IntentRequest intent)
            {
                return intent.Intent.Name == BuiltInIntent.Help;
            }

            return false;
        }

        public override SkillResponse HandleSyncRequest(RequestInformation information)
        {
            return ResponseBuilder.Ask(
                "This is pet match. I can help you find the perfect pet for you. You can say, I want a dog.",
                new Reprompt("What size and temperament are you looking for in a dog?"));
        }
    }
}
