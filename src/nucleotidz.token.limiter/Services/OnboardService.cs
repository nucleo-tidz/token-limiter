namespace nucleotidz.token.limiter.Services
{
    using System.Threading.Tasks;

    using nucleotidz.token.limiter.abstraction.Services;
    using nucleotidz.token.limiter.configuration.Model;
    using nucleotidz.token.limiter.Helpers;

    using StackExchange.Redis;

    internal sealed class OnboardService(IConnectionMultiplexer connectionMultiplexer, Utility utility) :
        RedisBase(connectionMultiplexer), IOnboardService
    {
        public async Task<bool> IsOnboardedAsync(string client)
        =>
           await base.Database.KeyExistsAsync(utility.GetClientConfigKey(client));
        public async Task OnBoardAsync(TokenLimitModel tokenLimitModel)
        =>
            await base.Database.StringSetAsync(utility.GetClientConfigKey(tokenLimitModel.client),
                System.Text.Json.JsonSerializer.Serialize(tokenLimitModel));
    }
}
