namespace Architecture.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.Extensions.DependencyModel;

    using NetArchTest.Rules;

    using nucleotidz.token.limiter;
    using nucleotidz.token.limiter.abstraction;

    public class LimiterTest
    {
        [Fact]
        public void limiter_assembly_should_not_have_interfaces()
        {
            var result = Types.InAssembly(typeof(DependencyInjection).Assembly)
                .Should()
                .NotBeInterfaces()
                .GetResult();
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void limiter_assembly_not_inhertit_interfaces()
        {
            var result = Types.InAssembly(typeof(DependencyInjection).Assembly)
               .That().ResideInNamespace("nucleotidz.token.limiter.Limiters")
                .Should().BeClasses()
                .And().NotBeAbstract()
                .And().BeSealed()
                .And().NotBePublic()
                .And().ImplementInterface(typeof(ITokenLimiter))
                .GetResult();
            Assert.True(result.IsSuccessful);
        }
        [Fact]
        public void base_class_should_not_be_Sealed()
        {
            var result = Types.InAssembly(typeof(DependencyInjection).Assembly)
               .That()
               .AreClasses()
               .And()
               .HaveNameMatching(".*Base$").Should().NotBeSealed().GetResult();

            Assert.True(result.IsSuccessful);
        }
    }
}
