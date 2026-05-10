namespace Architecture.Test
{
    using Mono.Cecil;

    using NetArchTest.Rules;

    using nucleotidz.token.limiter.configuration.Options;

    public class ConfigurationTest
    {
        [Fact]
        public void configuration_assembly_option_folder_should_have_classes()
        {
            var result = Types.InAssembly(typeof(RedisOption).Assembly)
               .That().ResideInNamespace("nucleotidz.token.limiter.configuration.Options")
               .Should().BeClasses()
               .And().BePublic()
                .And().HaveNameEndingWith("Option").GetResult();

            Assert.True(result.IsSuccessful);
        }
        [Fact]
        public void configuration_assembly_model_folder_should_have_classes()
        {
            var result = Types.InAssembly(typeof(RedisOption).Assembly)
               .That().ResideInNamespace("nucleotidz.token.limiter.configuration.TokenLimitModel")
               .Should()
              .BePublic()
              .And().HaveNameEndingWith("Model").GetResult();

            Assert.True(result.IsSuccessful);
        }
        [Fact]
        public void configuration_assembly_enum_folder_should_have_immutable()
        {
            var result = Types.InAssembly(typeof(RedisOption).Assembly)
               .That().ResideInNamespace("nucleotidz.token.limiter.configuration.Enums")
               .Should()
              .MeetCustomRule(new EnumRule())
              .GetResult();

            Assert.True(result.IsSuccessful);
        }

        public class EnumRule : ICustomRule
        {
            public bool MeetsRule(TypeDefinition type)
            {
                return type.IsEnum;
            }
        }
    }
}