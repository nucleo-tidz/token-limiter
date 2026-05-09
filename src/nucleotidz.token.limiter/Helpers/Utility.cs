namespace nucleotidz.token.limiter.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Utility
    {
        protected internal string GetClientConfigKey(string clientKey) => $"TokenLimiter:Config:{clientKey}";
        protected internal string GetLimitKey(string clientKey) => $"TokenLimiter:Limit:{clientKey}";
    }
}
