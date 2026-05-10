namespace Architecture.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using NetArchTest.Rules;

    using nucleotidz.token.limiter.abstraction;

    public class AbstractionTest
    {
        [Fact]
        public void abstraction_assembly_should_not_have_class()
        {
            var result = Types.InAssembly(typeof(ITokenLimiter).Assembly)
                .Should()
                .NotBeClasses()
                .GetResult();
            Assert.True(result.IsSuccessful);
        }
        [Fact]
        public void abstraction_assembly_service_interface_should_suffix_service()
        {
            var result = Types.InAssembly(typeof(ITokenLimiter).Assembly)
               .That().ResideInNamespace("nucleotidz.token.limiter.abstraction.Services")
                .Should().BeInterfaces()
                .And().HaveNameEndingWith("Service").GetResult();

            Assert.True(result.IsSuccessful);
        }
    }
}
