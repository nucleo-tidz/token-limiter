namespace nucleotidz.token.limiter.unittest
{
    using System.Text.Json;

    using Moq;

    using nucleotidz.token.limiter.configuration.Model;
    using nucleotidz.token.limiter.Helpers;
    using nucleotidz.token.limiter.Limiters;
    using StackExchange.Redis;
    public class FixedWindowLimiterTest
    {
        private readonly Mock<IConnectionMultiplexer> connectionMock;
        private readonly Mock<IDatabase> databaseMock;
        private readonly Utility utility;
        public FixedWindowLimiterTest()
        {
            connectionMock = new Mock<IConnectionMultiplexer>();
            databaseMock = new Mock<IDatabase>();

            connectionMock
                .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(databaseMock.Object);

            utility = new Utility();
        }
        [Fact]
        public async Task is_allowed_should_return_true_when_limit_not_exceeded()
        {
            var clientKey = "client-test";
            var model = new TokenLimitModel(clientKey, TimeSpan.FromMinutes(1), 100);
            databaseMock
                .Setup(x => x.StringGetAsync(
                    utility.GetClientConfigKey(clientKey),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(JsonSerializer.Serialize(model));
            databaseMock
                .Setup(x => x.StringGetAsync(
                    utility.GetLimitKey(clientKey),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync("50");
            var limiter = new FixedWindowLimiter(
                connectionMock.Object,
                utility);       
            var result = await limiter.IsAllowedAsync(clientKey);
            Assert.True(result);
        }

        [Fact]
        public async Task is_allowed_should_return_false_when_limit_exceeded()
        {
            var clientKey = "client-test";
            var model = new TokenLimitModel(clientKey, TimeSpan.FromMinutes(1), 100);

            databaseMock
                .Setup(x => x.StringGetAsync(
                    utility.GetClientConfigKey(clientKey),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(JsonSerializer.Serialize(model));

            databaseMock
                .Setup(x => x.StringGetAsync(
                    utility.GetLimitKey(clientKey),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync("150");

            var limiter = new FixedWindowLimiter(
                connectionMock.Object,
                utility); 
            var result = await limiter.IsAllowedAsync(clientKey);          
            Assert.False(result);
        }

        [Fact]
        public async Task should_throw_when_client_not_onboarded()
        {
            var clientKey = "client-test";
            databaseMock
                .Setup(x => x.StringGetAsync(
                    utility.GetClientConfigKey(clientKey),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            var limiter = new FixedWindowLimiter(
                connectionMock.Object,
                utility);       
            await Assert.ThrowsAsync<ApplicationException>(() =>
                limiter.IsAllowedAsync(clientKey));
        }
        [Fact]
        public async Task consume_should_set_expiry_for_first_request()
        {
            var clientKey = "client-test";
            var model = new TokenLimitModel(clientKey, TimeSpan.FromMinutes(1), 100);
            databaseMock
                .Setup(x => x.StringGetAsync(
                    utility.GetClientConfigKey(clientKey),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(JsonSerializer.Serialize(model));

            databaseMock
                .Setup(x => x.StringIncrementAsync(
                    utility.GetLimitKey(clientKey),
                    10,
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(10);

            var limiter = new FixedWindowLimiter(
                connectionMock.Object,
                utility);     
            await limiter.ConsumeAsync(clientKey, 10);
            databaseMock.Verify(x =>
                x.KeyExpireAsync(
                    utility.GetLimitKey(clientKey),
                    model.window,
                    It.IsAny<ExpireWhen>(),
                    It.IsAny<CommandFlags>()),
                Times.Once);
        }
    }

}

