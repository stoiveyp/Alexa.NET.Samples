using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.Response;
using Newtonsoft.Json;

namespace AlexaSamplePetMatch.RequestHandlers
{
    public class CompletedPetMatch:IRequestHandler
    {
        public bool CanHandle(RequestInformation information)
        {
            if (information.SkillRequest.Request is IntentRequest intent)
            {
                return intent.Intent.Name == Consts.PetMatchIntent
                       && intent.Intent.ConfirmationStatus == DialogState.Completed;
            }

            return false;
        }

        public async Task<SkillResponse> Handle(RequestInformation information)
        {
            var intent = ((IntentRequest) information.SkillRequest.Request).Intent;
            var responseMessage = await MakeApiCall(intent);
            var rawContent = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ApiResponse>(rawContent);

            return response.Results.Length > 0 ? Match(intent.Slots, response.Results.First().Breed) : NoMatch(intent.Slots);
        }

        private SkillResponse Match(Dictionary<string, Slot> slots, string breed)
        {
            //i want a hairy low energy dog
            return ResponseBuilder.Tell($"Okay, so a {DogDescription(slots)} sounds good to you. Consider a {breed}");
        }

        private SkillResponse NoMatch(Dictionary<string, Slot> slots)
        {
            return ResponseBuilder.Tell($"I'm sorry, I couldn't find {DogDescription(slots)}");
        }

        private string DogDescription(Dictionary<string, Slot> slots)
        {
            return $"a {Resolved(slots["size"])} {Resolved(slots["temperament"])} {Resolved(slots["energy"])} energy dog";
        }

        private string ApiQuery(Dictionary<string, Slot> slots)
        {
            return $"SSET={Resolved(slots["energy"])}-{Resolved(slots["size"])}-{Resolved(slots["temperament"])}";
        }

        private string Resolved(Slot slot)
        {
            return slot.Resolution.Authorities.First().Values.First().Value.Name;
        }

        private Task<HttpResponseMessage> MakeApiCall(Intent intent)
        {
            var client = new HttpClient();
            var uri = GenerateUrl(intent.Slots);
            return client.GetAsync(uri);
        }

        private Uri GenerateUrl(Dictionary<string, Slot> slots)
        {
            var host = "e4v7rdwl7l.execute-api.us-east-1.amazonaws.com";
            var builder = new UriBuilder("https", host)
            {
                Path = "/Test",
                Query = ApiQuery(slots)
            };
            Console.WriteLine(builder.Uri.ToString());
            return builder.Uri;
        }
    }
}
