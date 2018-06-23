using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace AlexaSamplePetMatch.RequestHandlers
{
    public class Exit:SynchronousRequestHandler
    {
        public override bool CanHandle(RequestInformation information)
        {
            if (information.SkillRequest.Request is IntentRequest intent)
            {
                return intent.Intent.Name == BuiltInIntent.Stop || intent.Intent.Name == BuiltInIntent.Cancel;
            }

            return false;
        }

        public override SkillResponse HandleSyncRequest(RequestInformation information)
        {
            return ResponseBuilder.Tell("Bye");
        }
    }
}
