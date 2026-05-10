namespace architecture.test
{
    using NetArchTest.Rules;



    using Xunit;

    public class DependencyTest
    {
        [Fact]
        public void configuration_assesmbly_not_depend_on_main()
        {
            var result = Types.InNamespace("nucleotidz.token.limiter.configuration")
                .Should()
                .NotHaveDependencyOnAny("nucleotidz.token.limiter")
                .GetResult().IsSuccessful;

            Assert.True(result);
        }

        [Fact]
        public void abstraction_assesmbly_not_depend_on_main()
        {
            var result = Types.InNamespace("nucleotidz.token.limiter.abstraction")
                .Should()
                .NotHaveDependencyOnAny("nucleotidz.token.limiter")
                .GetResult().IsSuccessful;

            Assert.True(result);
        }

        [Fact]
        public void main_assesmbly_not_depend_on_abstraction()
        {
            var result = Types.InNamespace("nucleotidz.token.limiter")
                .Should()
                .HaveDependencyOnAny("nucleotidz.token.limiter.configuration")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

    }
}
