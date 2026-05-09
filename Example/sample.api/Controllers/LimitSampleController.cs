namespace sample.api.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using nucleotidz.token.limiter.abstraction;
    using nucleotidz.token.limiter.abstraction.Services;
    using nucleotidz.token.limiter.configuration.Model;
    using nucleotidz.token.limiter.Filter;

    [Route("api/[controller]")]
    [ApiController]

    public class LimitSampleController(IOnboardService onboardService, ITokenLimiter tokenLimiter) : ControllerBase
    {
        [HttpGet("get")]
        [ServiceFilter(typeof(AITokenRateLimiter))]
        public async Task<IActionResult> Get([FromHeader(Name = "x-ai-client-key")] string clientkey)
        {
            return Ok("Request successful");
        }

        [HttpPost("onboard")]
        public async Task<IActionResult> Onboard(TokenLimitModel tokenLimitModel)
        {
            await onboardService.OnBoardAsync(tokenLimitModel);
            return Ok("User onboarded successfully.");
        }


        [HttpGet("consume/{tokens}")]
        public async Task<IActionResult> consume([FromHeader(Name = "x-ai-client-key")] string clientkey, long tokens)
        {
            // In actual Implementaion calle this method after the AI request is processed successfully and token consumption is available
            await tokenLimiter.ConsumeAsync(clientkey, tokens);
            return Ok("Request successful");
        }
    }
}
