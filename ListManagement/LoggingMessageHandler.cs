using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ListManagement
{
    internal class LoggingMessageHandler : DelegatingHandler
    {
        public StringBuilder CurrentLogs { get; }

        public LoggingMessageHandler()
        {
            CurrentLogs = new StringBuilder();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CurrentLogs.AppendLine($"request: {request.RequestUri}");
            var result = await base.SendAsync(request, cancellationToken);
            var output = await result.Content.ReadAsStringAsync();
            CurrentLogs.AppendLine(output);
            return result;
        }
    }
}