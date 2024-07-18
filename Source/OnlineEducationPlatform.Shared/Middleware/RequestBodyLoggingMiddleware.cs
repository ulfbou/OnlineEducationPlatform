using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OnlineEducationPlatform.Shared.Middleware
{
    public class RequestBodyLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestBodyLoggingMiddleware> _logger;

        public RequestBodyLoggingMiddleware(RequestDelegate next, ILogger<RequestBodyLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var originalBodyStream = context.Request.Body;
            var loggingStream = new LoggingStream(originalBodyStream, _logger);
            context.Request.Body = loggingStream;

            await _next(context);

            // Reset the request body stream position to ensure downstream components can read it
            context.Request.Body.Position = 0;
        }
    }
}