using System.Collections.Generic;
using Alexa.NET.RequestHandlers;
using AlexaSamplePetMatch.RequestHandlers;

namespace AlexaSamplePetMatch
{
    public class RequestManager:Request
    {
        public RequestManager() : base(new IRequestHandler[]
        {
            new LaunchRequest(),
            new MythicalCreature(),
            new InProgressPetMatch(),
            new CompletedPetMatch()
        },new IErrorHandler[]{new ErrorHandler()}){}
    }
}