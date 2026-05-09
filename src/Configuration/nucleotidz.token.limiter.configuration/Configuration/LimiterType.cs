namespace nucleotidz.token.limiter.configuration
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
