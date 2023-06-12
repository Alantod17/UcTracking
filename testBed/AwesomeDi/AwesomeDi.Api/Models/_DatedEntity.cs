using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeDi.Api.Models
{
    public class DatedEntity
    {
        public DateTime? CreatedUtcDateTime { get; set; }
        public DateTime? ModifiedUtcDateTime { get; set; }
    }
}
