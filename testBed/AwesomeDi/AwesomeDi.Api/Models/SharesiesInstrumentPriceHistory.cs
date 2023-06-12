using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeDi.Api.Models
{
	public class SharesiesInstrumentPriceHistory : DatedEntity
	{
		[Key] public int Id { get; set; }
		[Required] public DateTime Date { get; set; }
		[Required] public decimal Price { get; set; }
        [Required] public int SharesiesInstrumentId { get; set; }
		public SharesiesInstrument SharesiesInstrument { get; set; }
	}
}
