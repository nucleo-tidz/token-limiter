namespace nucleotidz.token.limiter.configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RedisOption
    {
        public const string SectionName = "Redis";
        public required string EndPoints { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int SyncTimeout { get; set; } = 60000;
    }
}
