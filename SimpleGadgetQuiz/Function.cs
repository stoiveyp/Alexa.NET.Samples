using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Gadgets.GameEngine;
using Alexa.NET.Gadgets.GameEngine.Requests;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.Ssml;
using Alexa.NET.StateManagement;
using Alexa.NET.Response.Ssml.SoundLibrary;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SimpleGadgetQuiz
{
    public class Function
    {
        public Function()
        {
            new GadgetRequestHandler().AddToRequestConverter();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var state = new SkillState(input);

            switch (input.Request)
            {
                case LaunchRequest _:
                    var rollCallResponse = ResponseBuilder.Ask("Please press the buttons you want to use for this quiz", null);
                    rollCallResponse.AddRollCall(20000, "first", "second");

                    return rollCallResponse;
                case InputHandlerEventRequest inputHandler:
                    if (inputHandler.TryRollCallResult(out var mapping, "first", "second"))
                    {
                        state.SetSession("player1", mapping["first"]);
                        state.SetSession("player2", mapping["second"]);
                        var question = "Question One. Which is the bigger number, 3 or 5?";

                        var response = ResponseBuilder.Ask($"Okay then, let's begin! {question}", null,state.Session);
                        response.WhenFirstButtonDown(mapping, "buzzedIn", 10000);
                        response.Response.ShouldEndSession = null;
                        return response;
                    }

                    if (inputHandler.TryMapEventGadget("buzzedIn", out var player))
                    {
                        var buzzedInPlayer = player == state.GetSession<string>("player1") ? "player one" : "player two";
                        state.SetSession("currentAnswer", "five");
                        return ResponseBuilder.Ask($"Okay then {buzzedInPlayer}, what's your answer?",null, state.Session);
                    }

                    break;
                case IntentRequest intent:
                    switch (intent.Intent.Name)
                    {
                        case BuiltInIntent.Cancel: case BuiltInIntent.Stop:
                            return ResponseBuilder.Empty();
                        case "answer":
                            Speech responseSpeech = await CheckAnswer(intent,state) ?
                                new Speech(Human.CrowdCheerMedium, new PlainText("Well done, that's correct!")) :
                                new Speech(Human.CrowdBoo01, new PlainText("Sorry, that's not the right answer"));

                            return ResponseBuilder.Tell(responseSpeech);
                    }

                    break;
            }

            return ResponseBuilder.Tell("Sorry - I'm not sure how to handle that");
        }

        private async Task<bool> CheckAnswer(IntentRequest intent, SkillState state)
        {
            var expected = await state.Get<string>("currentAnswer");
            var slot = intent.Intent.Slots["currentAnswer"];
            var resolution = slot.Resolution.Authorities.First();
            return slot.Value == expected || 
                (resolution.Status.Code == ResolutionStatusCode.SuccessfulMatch && 
                 resolution.Values.First().Value.Name == expected);
        }
    }
}
