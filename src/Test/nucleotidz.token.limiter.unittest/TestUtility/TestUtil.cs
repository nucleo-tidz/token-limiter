namespace nucleotidz.token.limiter.unittest.TestUtility
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.Extensions.DependencyInjection;

    public static class TestUtil
    {
        public static void RemoveService<T>(this IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        public static Dictionary<string, string> InMemoryConfiguration
     
            => new Dictionary<string, string>
            {
                {"Redis:EndPoints", "localhost"},
                {"Redis:Port", "6379"},
                {"Redis:UserName", ""},
                {"Redis:Password", ""},
                {"Redis:SyncTimeout", "5000"}
            };
        
    }
}
