namespace nucleotidz.token.limiter.configuration.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public record TokenLimitModel([Required] string client, [Required] TimeSpan window, [Required] long limit);

}
