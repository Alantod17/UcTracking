using System.ComponentModel.DataAnnotations;

namespace AwesomeDi.Api.Models
{
	public class SharesiesInstrumentComparisonPrice : DatedEntity
	{
        public enum TypeEnum
        {
            Hour = 10,
            ThreeHour = 20,
            SixHour = 30,
            TwelveHour = 50,
            Day = 100,
            ThreeDay = 110,
            Week = 200,
            TwoWeek = 210,
            Month = 300,
            TwoMonth = 310,
            ThreeMonth = 320,
            SixMonth = 330,
            Year = 400,
            TwoYear = 410,
        }
		[Key] public int Id { get; set; }
        public TypeEnum Type { get; set; }
        public decimal Value { get; set; }
        public decimal Percent { get; set; }
        public decimal Max { get; set; }
        public decimal Min { get; set; }
        [Required] public int SharesiesInstrumentId { get; set; }
        public SharesiesInstrument SharesiesInstrument { get; set; }
    }
}