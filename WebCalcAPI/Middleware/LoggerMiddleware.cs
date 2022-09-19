namespace WebCalcAPI.Middleware;

using Microsoft.AspNetCore.Http.Extensions;
public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggerMiddleware> _logger;
    public LoggerMiddleware(RequestDelegate request, ILogger<LoggerMiddleware> logger)
    {
        _next = request;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestBody = context.Request.Body;

        var url = context.Request.GetDisplayUrl();
        using (var requestStreamReader = new StreamReader(requestBody))
        {
            var requestBodyString = await requestStreamReader.ReadToEndAsync();
            _logger.LogInformation(
                $"REQUEST_SERVICE: {nameof(context.TraceIdentifier)}: {context.TraceIdentifier}, " +
                $"Method: {context.Request.Method}, RequestUri: '{url}', " +
                $"Headers: {string.Join(" | ", context.Request.Headers.Select(header => $"{header.Key}:{string.Join(", ", header.Value)}"))}, " +
                $"Body: {requestBodyString}");
        }


        await _next(context);
        context.Request.Body = requestBody;

    }

}

