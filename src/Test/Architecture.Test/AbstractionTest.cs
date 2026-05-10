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
        public void Abstraction_assembly_should_not_have_class()
        {
            var result = Types.InAssembly(typeof(ITokenLimiter).Assembly)
                .Should()
                .NotBeClasses()
                .GetResult();
            Assert.True(result.IsSuccessful);
        }
    }
}
