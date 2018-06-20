using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Response;

namespace AlexaSamplePetMatch
{
    public static class RepromptHelper
    {
        public static Reprompt From(string text)
        {
            return new Reprompt{OutputSpeech = new PlainTextOutputSpeech{Text=text}};
        }
    }
}
