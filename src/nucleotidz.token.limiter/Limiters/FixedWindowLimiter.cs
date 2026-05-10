namespace nucleotidz.token.limiter.Limiters
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    using nucleotidz.token.limiter.abstraction;
    using nucleotidz.token.limiter.configuration.Model;
    using nucleotidz.token.limiter.Helpers;

    using StackExchange.Redis;

    internal sealed class FixedWindowLimiter
        : RedisBase, ITokenLimiter
    {
        private readonly Utility utility;
        private readonly IDatabase db;
        public  FixedWindowLimiter(IConnectionMultiplexer _connectionMultiplexer, Utility _utility) : base(_connectionMultiplexer)
        {
            utility = _utility;
            db = base.Database;
        }
        public async Task ConsumeAsync(string clientKey, long tokens)
        {
            var tokenLimitModel = await GetClientConfig(clientKey);
            string clientLimitKey = utility.GetLimitKey(clientKey);
            long current = await db.StringIncrementAsync(clientLimitKey, tokens);
            if (current == tokens)
            {
                await db.KeyExpireAsync(clientLimitKey, tokenLimitModel.window);
            }
        }
        public async Task<bool> IsAllowedAsync(string clientKey)
        {
            var tokenLimitModel = await GetClientConfig(clientKey);
            var raw = await db.StringGetAsync(utility.GetLimitKey(clientKey));
            long clientConsumed = raw.HasValue ? (long)raw : 0;
            return !(clientConsumed > tokenLimitModel.limit);
        }
        private async Task<TokenLimitModel> GetClientConfig(string clientKey)
        {
            var clientConfiguration = await db.StringGetAsync(utility.GetClientConfigKey(clientKey));
            if (string.IsNullOrEmpty(clientConfiguration))
                throw new ApplicationException($"Client : {clientKey} is not on boarded yet");
            return JsonSerializer.Deserialize<TokenLimitModel>(clientConfiguration.ToString());
        }
    }
}
