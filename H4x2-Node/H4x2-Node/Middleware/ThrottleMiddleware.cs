using H4x2_Node.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace H4x2_Node.Middleware;

public class ThrottleMiddleware
{
    private readonly RequestDelegate _next;
    private ThrottlingManager _throttlingManager;

    public ThrottleMiddleware(RequestDelegate next)
    {
        _next = next;
        _throttlingManager = new ThrottlingManager();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var barredTime = GetBarredTime(context).Value;
        if (!barredTime.Equals(0)) // if throttled
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsync(barredTime.ToString()); // need to fix this so that is returns a header with this info. Enclave needs to be hosted on ork for this
        }
        else
        {
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        } 
    }
    private ActionResult<int> GetBarredTime(HttpContext context)
    {
        var Ip = (context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "").Split(new char[] { ':' }).FirstOrDefault(); // in case you're using proxy or load balancer
        if (!String.IsNullOrEmpty(Ip))
            return _throttlingManager.Throttle(Ip.ToString()).GetAwaiter().GetResult();
        else
        {
            Console.WriteLine("Throttled IP address (NOT Forwarded): " + context.Connection.RemoteIpAddress.ToString());
            return _throttlingManager.Throttle(context.Connection.RemoteIpAddress.ToString()).GetAwaiter().GetResult();

        }
    }
}

public static class RequestCultureMiddlewareExtensions
{
    public static IApplicationBuilder UseThrottling(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ThrottleMiddleware>();
    }
}