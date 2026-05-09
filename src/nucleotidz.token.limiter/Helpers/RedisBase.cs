namespace nucleotidz.token.limiter.Helpers
{
    using StackExchange.Redis;

    internal class RedisBase(IConnectionMultiplexer connectionMultiplexer)
    {
        internal IDatabase Database => connectionMultiplexer.GetDatabase();
    }
}
