using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace AlexaSamplePetMatch.RequestHandlers
{
    public class InProgressPetMatch : SynchronousRequestHandler
    {
        public override bool CanHandle(RequestInformation information)
        {
            if (information.SkillRequest.Request is IntentRequest intent)
            {
                return intent.Intent.Name == Consts.PetMatchIntent
                       && intent.Intent.ConfirmationStatus != DialogState.Completed;
            }

            return false;
        }

        public override SkillResponse HandleSyncRequest(RequestInformation information)
        {
            var intent = ((IntentRequest)information.SkillRequest.Request).Intent;

            foreach (var slot in intent.Slots.Values)
            {
                if (
                    slot.ConfirmationStatus != ConfirmationStatus.Confirmed
                    && (slot.Resolution?.Authorities.Any() ?? false)
                )
                {
                    var authority = slot.Resolution.Authorities.First();
                    switch (authority.Status.Code)
                    {
                        case ResolutionStatusCode.SuccessfulMatch when authority.Values.Length > 1:
                            var response = PickOneResponse(authority);
                            return ResponseBuilder.DialogElicitSlot(
                                new PlainTextOutputSpeech{Text=response}, 
                                slot.Name,information.State.Session,intent);
                        case ResolutionStatusCode.NoMatch:
                            var prompt = $"What {slot.Name} are you looking for?";
                            return ResponseBuilder.DialogElicitSlot(
                                new PlainTextOutputSpeech { Text = prompt },
                                slot.Name, information.State.Session, intent);
                    }
                }
            }

            return ResponseBuilder.DialogDelegate(intent);
        }

        private string PickOneResponse(ResolutionAuthority authority)
        {
            var sb = new StringBuilder("Which would you like,");

            foreach (var value in authority.Values.Take(authority.Values.Length - 2))
            {
                sb.Append(" ");
                sb.Append(value.Value.Name);
                sb.Append(" ");
            }

            sb.Append("or ");
            sb.Append(authority.Values.Last().Value);
            sb.Append("?");
            return sb.ToString();
        }
    }
}
