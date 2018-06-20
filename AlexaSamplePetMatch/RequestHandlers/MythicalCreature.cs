using System;
using System.Linq;
using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace AlexaSamplePetMatch.RequestHandlers
{
    public class MythicalCreature : SynchronousRequestHandler
    {
        private const string SlotName = "pet";
        private const string ResolutionValue = "mythical_creatures";
        private const string CreatureKey = "mythicalCreature";

        private readonly string[] Responses = {
            "I'm sorry, but I'm not qualified to match you with {0}s.",
            "Ah yes, {0}s are splendid creatures, but unfortunately owning one as a pet is outlawed.",
            "I'm sorry I can't match you with {0}s."
        };

        public override bool CanHandle(RequestInformation information)
        {
            var intentRequest = information.SkillRequest.Request as IntentRequest;
            if (intentRequest == null || intentRequest.Intent.Name != Consts.PetMatchIntent)
            {
                return false;
            }

            if (!intentRequest.Intent.Slots.ContainsKey(SlotName))
            {
                return false;
            }

            var petSlot = intentRequest.Intent.Slots[SlotName];
            var isMythical = petSlot?.Resolution?.Authorities.FirstOrDefault()?
                .Values.FirstOrDefault()?.Value.Name == ResolutionValue;

            if (isMythical)
            {
                information.State.SetRequest(CreatureKey, petSlot.Value);
                return true;
            }

            return false;
        }

        public override SkillResponse HandleSyncRequest(RequestInformation information)
        {
            var rnd = new Random();
            var phraseNumber = rnd.Next(0, Responses.Length - 1);
            return ResponseBuilder.Tell(
                string.Format(Responses[phraseNumber],
                    information.State.GetRequest<string>(CreatureKey
                    )));
        }
    }
}
