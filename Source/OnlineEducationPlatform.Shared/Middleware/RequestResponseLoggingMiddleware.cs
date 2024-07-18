using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OnlineEducationPlatform.Shared.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request
            context.Request.EnableBuffering();
            var request = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;
            _logger.LogInformation($"Request Headers: {System.Text.Json.JsonSerializer.Serialize(context.Request.Headers)}");
            _logger.LogInformation($"Request Body: {request}");

            // Intercept response
            var originalBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            // Log response
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;

            _logger.LogInformation($"Response Headers: {System.Text.Json.JsonSerializer.Serialize(context.Response.Headers)}");
            _logger.LogInformation($"Response Body: {response}");
        }
    }
}