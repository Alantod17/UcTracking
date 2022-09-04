using System.ComponentModel.DataAnnotations;

namespace AwesomeDi.Api.Models
{
	public class SharesiesInstrumentXCategory : DatedEntity
	{
		[Key] public int Id { get; set; }
        [Required] public int SharesiesInstrumentId { get; set; }
        public SharesiesInstrument SharesiesInstrument { get; set; }
        [Required] public int SharesiesInstrumentCategoryId { get; set; }
        public SharesiesInstrumentCategory SharesiesInstrumentCategory { get; set; }
    }
}
