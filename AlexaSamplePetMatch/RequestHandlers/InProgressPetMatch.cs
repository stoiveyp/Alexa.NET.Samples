using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using Alexa.NET.StateManagement;

namespace AlexaSamplePetMatch.RequestHandlers
{
    public class InProgressPetMatch : SynchronousRequestHandler
    {
        private readonly string[] RequiredSlots = {"energy","size","temperament"};

        public override bool CanHandle(RequestInformation information)
        {
            if (information.SkillRequest.Request is IntentRequest intent && intent.Intent.Name == Consts.PetMatchIntent)
            {
                if (intent.DialogState == DialogState.Completed)
                {
                    return false;
                }

                if (intent.DialogState == DialogState.Started)
                {
                    LoadIntent(intent.Intent,information.State);
                }

                return true;
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
                            SaveIntent(intent, information.State);
                            return ResponseBuilder.DialogElicitSlot(
                                new PlainTextOutputSpeech{Text=response}, 
                                slot.Name,information.State.Session,intent);
                        case ResolutionStatusCode.NoMatch when RequiredSlots.Contains(slot.Name):
                            var prompt = $"What {slot.Name} are you looking for?";
                            SaveIntent(intent, information.State);
                            return ResponseBuilder.DialogElicitSlot(
                                new PlainTextOutputSpeech { Text = prompt },
                                slot.Name, information.State.Session, intent);
                    }
                }
            }

            SaveIntent(intent, information.State);
            return ResponseBuilder.DialogDelegate(intent);
        }

        private void SaveIntent(Intent intent, ISkillState state)
        {
            if (intent?.Slots?.Any() ?? false)
            {
                Console.WriteLine("Saving intent: " + string.Join(",",intent.Slots.Values.Where(s => !string.IsNullOrWhiteSpace(s.Value)).Select(s =>s.Name)));
                state.SetSession("temp_petmatch",intent.Slots);
            }
        }

        private void LoadIntent(Intent intent, ISkillState state)
        {
            var slots = state.GetSession<Dictionary<string, Slot>>("temp_petmatch");
            if (slots != default(Dictionary<string, Slot>))
            {
                if (intent.Slots == null)
                {
                    intent.Slots = new Dictionary<string, Slot>();
                }

                foreach (var slot in slots.Where(s => s.Value.Value != null).Select(kvp => kvp.Value))
                {
                    if (intent.Slots.ContainsKey(slot.Name))
                    {
                        if (intent.Slots[slot.Name].Value != null)
                        {
                            continue;
                        }

                        intent.Slots.Remove(slot.Name);
                    }
                    Console.WriteLine("loading slot " + slot.Name);
                    intent.Slots.Add(slot.Name,slot);
                }

                intent.Slots = slots;
            }
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
