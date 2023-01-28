using System.Diagnostics;

namespace ResolvingDeps.WebApi.Middlewares;

// Interface IMiddleware also exists, but it limits flexibility into what we can DI into our middleware
public class DurationLoggerMiddleware : IMiddleware
{
    // private readonly RequestDelegate _next;
    // without using IMiddleware
    // public DurationLoggerMiddleware(RequestDelegate next,
    //     ILogger<DurationLoggerMiddleware> logger)
    // {
    //     _next = next;
    //     _logger = logger;
    // }
    //
    // public async Task InvokeAsync(HttpContext context)
    // {
    //     var sw = Stopwatch.StartNew();
    //     try
    //     {
    //         await _next(context);
    //     }
    //     finally
    //     {
    //         var text = $"Request completed in {sw.ElapsedMilliseconds}ms";
    //         _logger.LogInformation(text);
    //     }
    // }
    
    private readonly ILogger<DurationLoggerMiddleware> _logger;

    public DurationLoggerMiddleware(ILogger<DurationLoggerMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await next(context);
        }
        finally
        {
            var text = $"Request completed in {sw.ElapsedMilliseconds}ms";
            _logger.LogInformation(text);
        }
    }
}
