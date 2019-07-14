using System.Collections.Generic;
using Alexa.NET;
using Alexa.NET.APL;
using Alexa.NET.APL.Commands;
using Alexa.NET.APL.Components;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace APLTransition
{
    public class Function
    {
        public SkillResponse FunctionHandler(APLSkillRequest input, ILambdaContext context)
        {
            var response = ResponseBuilder.Tell("Here's your transition");
            response.Response.Directives.Add(new RenderDocumentDirective
            {
                Document = GetDocument()
            });
            return response;
        }

        private APLDocument GetDocument()
        {

            var definition = JsonConvert.DeserializeObject<CommandDefinition>(System.IO.File.ReadAllText("JackInTheBox.json"));

            return new APLDocument(APLDocumentVersion.V1_1)
            {
                Commands = new Dictionary<string, CommandDefinition>
                {
                    {"JackInTheBox",definition }
                },
                MainTemplate = new Layout(
                    new Container
                    {
                        Height = new AbsoluteDimension(100, "vh"),
                        Width = new AbsoluteDimension(100, "vw"),
                        AlignItems = "center",
                        Direction = "column",
                        JustifyContent = "center",
                        Items = new List<APLComponent>
                        {
                            new Text("APL Transition!")
                            {
                                FontSize = 70,
                                Height = new AbsoluteDimension(15,"vh"),
                                Width = new AbsoluteDimension(47,"vw"),
                                OnMount = new List<APLCommand>
                                {
                                    new CustomCommand("JackInTheBox")
                                    {
                                        ParameterValues = new Dictionary<string, object>
                                        {
                                            {"duration",2000 }
                                        }
                                    }
                                }
                            }
                        }
                    }).AsMain()
            };
        }
    }
}
