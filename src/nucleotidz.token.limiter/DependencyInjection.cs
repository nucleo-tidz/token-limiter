namespace nucleotidz.token.limiter
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using nucleotidz.token.limiter.abstraction;
    using nucleotidz.token.limiter.abstraction.Services;
    using nucleotidz.token.limiter.configuration;
    using nucleotidz.token.limiter.configuration.Enums;
    using nucleotidz.token.limiter.configuration.Options;
    using nucleotidz.token.limiter.Filter;
    using nucleotidz.token.limiter.Helpers;
    using nucleotidz.token.limiter.Limiters;
    using nucleotidz.token.limiter.Services;

    using StackExchange.Redis;

    public static class DependencyInjection
    {
        public static IServiceCollection AddAITokenLimiterFilter(this IServiceCollection services)
        {
            return services
                .AddScoped<AITokenRateLimiter>();
        }
        public static IServiceCollection AddAITokenLimiter(this IServiceCollection services, IConfiguration configuration, LimiterType limiterType)
        {
            services.AddRedis(configuration)
                .AddTransient<Utility>()
                .AddScoped<IOnboardService,OnboardService>();
            _ = limiterType switch
            {
                LimiterType.FixedWindow => services.AddFixedWindowLimiter(),
                LimiterType.SlidingWindow => services.AddSlidingWindowLimiter(),
                LimiterType.TokenBucket => services.AddTokenBucketLimiter(),
                _ => throw new ArgumentOutOfRangeException(nameof(limiterType), $"Unsupported limiter type: {limiterType}")
            };
            return services;
        }

        private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<RedisOption>()
                .BindConfiguration(RedisOption.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var options = _.GetRequiredService<IOptions<RedisOption>>().Value;
                return ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    EndPoints = { { options.EndPoints, options.Port } },
                    User = options.UserName,
                    Password = options.Password,
                    SyncTimeout = options.SyncTimeout
                });
            });

            return services;
        }
        private static IServiceCollection AddFixedWindowLimiter(this IServiceCollection services)
        => services.AddScoped<ITokenLimiter, FixedWindowLimiter>();
        private static IServiceCollection AddSlidingWindowLimiter(this IServiceCollection services)
        => throw new NotImplementedException("Sliding window limiter is not implemented yet.");
        private static IServiceCollection AddTokenBucketLimiter(this IServiceCollection services)
        => throw new NotImplementedException("Token bucket limiter is not implemented yet.");
    }
}
