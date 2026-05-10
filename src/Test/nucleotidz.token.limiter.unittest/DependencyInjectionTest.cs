using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using nucleotidz.token.limiter;
using nucleotidz.token.limiter.abstraction;
using nucleotidz.token.limiter.abstraction.Services;
using nucleotidz.token.limiter.configuration;
using nucleotidz.token.limiter.Helpers;
using nucleotidz.token.limiter.Limiters;
using nucleotidz.token.limiter.Services;
using nucleotidz.token.limiter.unittest.TestUtility;

using StackExchange.Redis;

namespace nucleotidz.token.limiter.unittest
{
    public class DependencyInjectionTest
    {
        [Fact]
        public void add_ai_token_limiter_fixed_Window_limiter()
        {
            var inMemorySettings =TestUtil.InMemoryConfiguration;
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton(configuration);
            services.AddAITokenLimiter(configuration, LimiterType.FixedWindow);           
            TestUtil.RemoveService<IConnectionMultiplexer>(services);
            var redisMock = new Mock<IConnectionMultiplexer>();
            services.AddSingleton(redisMock.Object);
            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider.GetService<IOnboardService>());
            Assert.NotNull(provider.GetService<ITokenLimiter>());
            Assert.NotNull(provider.GetService<Utility>());           
            Assert.IsType<FixedWindowLimiter>(provider.GetService<ITokenLimiter>());
        }

        [Fact]
        public void add_ai_token_limiter_fixed_Window_limiter_fail_throws_when_endpoint_not_provided()
        {
            var inMemorySettings = TestUtil.InMemoryConfiguration;
            inMemorySettings.Remove("Redis:EndPoints");
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton(configuration);
            services.AddAITokenLimiter(configuration, LimiterType.FixedWindow);

            var provider = services.BuildServiceProvider();

            Assert.Throws<Microsoft.Extensions.Options.OptionsValidationException>(() => provider.GetService<IConnectionMultiplexer>());

        }

        [Theory]
        [InlineData(LimiterType.SlidingWindow)]
        [InlineData(LimiterType.TokenBucket)]
        public void add_ai_token_limiter_fixed_Window_limiter_throws_when_unimplemented_limiters_used(LimiterType limiterType)
        {
            var inMemorySettings = TestUtil.InMemoryConfiguration;
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();

            Assert.Throws<NotImplementedException>(() =>
                services.AddAITokenLimiter(configuration, limiterType)
            );
        }
    }
}
