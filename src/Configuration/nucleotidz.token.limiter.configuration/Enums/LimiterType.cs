namespace nucleotidz.token.limiter.configuration.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum LimiterType
    {
        FixedWindow,
        SlidingWindow,
        TokenBucket,
    }
}
