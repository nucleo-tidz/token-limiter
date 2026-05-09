namespace nucleotidz.token.limiter.abstraction
{
    using System.ComponentModel.DataAnnotations;

    public interface ITokenLimiter
    {
        Task<bool> IsAllowedAsync([Required] string clientKey);
        Task ConsumeAsync([Required] string clientKey, [Required] long tokens);
    }
}
