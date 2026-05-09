namespace nucleotidz.token.limiter.abstraction.Services
{
    using System.ComponentModel.DataAnnotations;

    using nucleotidz.token.limiter.configuration.Model;

    public interface IOnboardService
    {
        Task OnBoardAsync([Required] TokenLimitModel tokenLimitModel);
        Task<bool> IsOnboardedAsync([Required] string client);
    }
}
