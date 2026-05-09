namespace nucleotidz.token.limiter.Filter
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    using nucleotidz.token.limiter.abstraction;
    using nucleotidz.token.limiter.abstraction.Services;

    internal class TokenRateLimiter(ITokenLimiter tokenLimiter, IOnboardService onboardService) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Request.Headers.TryGetValue("x-ai-client-key", out var clientKey);
            if(string.IsNullOrEmpty(clientKey))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.HttpContext.Response.WriteAsync("Missing required header : x-ai-client-key");
                return;
            }
            if (!await onboardService.IsOnboardedAsync(clientKey))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                await context.HttpContext.Response.WriteAsync($"Client :{clientKey} is not on boarded yet");
                return;
            }
            if (!await tokenLimiter.IsAllowedAsync(clientKey))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync($"Client :{clientKey} has exceeded the allowed request limit , please try after sometime");
                return;
            }
            await next();
        }
    }
}
