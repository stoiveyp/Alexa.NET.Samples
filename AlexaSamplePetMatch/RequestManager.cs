using Alexa.NET.RequestHandlers;
using AlexaSamplePetMatch.RequestHandlers;

namespace AlexaSamplePetMatch
{
    public class RequestManager:Request
    {
        public RequestManager() : base(new[]
        {
            new LaunchRequestHandler(),
            new MythicalCreatureHandler()
        }){}
    }
}