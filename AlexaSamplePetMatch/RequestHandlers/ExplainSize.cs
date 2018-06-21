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
    public class ExplainSize:SynchronousRequestHandler
    {
        public override bool CanHandle(RequestInformation information)
        {
            return information.SkillRequest.Request is IntentRequest intent 
                   && intent.Intent.Name == "ExplainSizeIntent";
        }

        public override SkillResponse HandleSyncRequest(RequestInformation information)
        {
            var intent = (IntentRequest) information.SkillRequest.Request;
            var size = intent.Intent.Slots["size"].Value;
            var dogSize = SizeChart.Sizes[size];

            var unit = intent.Intent.Slots["unitOfMeasurement"].Value;
            var description = unit == "pounds" ? dogSize.Pounds : dogSize.Kilograms;

            return ResponseBuilder.Ask($"a {size} dog is {description} {unit}. What size dog would you like?",null);
        }
    }
}
