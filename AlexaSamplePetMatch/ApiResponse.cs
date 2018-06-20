using Newtonsoft.Json;

namespace AlexaSamplePetMatch
{
    public class ApiResponse
    {
        [JsonProperty("result")]
        public Result[] Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("breed")]
        public string Breed { get; set; }
    }
}
