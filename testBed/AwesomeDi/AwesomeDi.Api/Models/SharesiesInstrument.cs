using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeDi.Api.Models
{
	public class SharesiesInstrument : DatedEntity
	{
        public SharesiesInstrument()
        {
            SharesiesInstrumentComparisonPriceList = new List<SharesiesInstrumentComparisonPrice>();
            SharesiesInstrumentPriceHistoryList = new List<SharesiesInstrumentPriceHistory>();
            SharesiesInstrumentXCategoryList = new List<SharesiesInstrumentXCategory>();
        }
		[Key] public int Id { get; set; }
        public string InstrumentType { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int RiskRating { get; set; }
        public string ExchangeCountry { get; set; }
        public string SharesiesId { get; set; }
        public string UrlSlug { get; set; }
        
        [InverseProperty(nameof(SharesiesInstrumentPriceHistory.SharesiesInstrument))]
        public List<SharesiesInstrumentPriceHistory> SharesiesInstrumentPriceHistoryList { get; set; }

        [InverseProperty(nameof(SharesiesInstrumentComparisonPrice.SharesiesInstrument))]
        public List<SharesiesInstrumentComparisonPrice> SharesiesInstrumentComparisonPriceList { get; set; }

        [InverseProperty(nameof(SharesiesInstrumentXCategory.SharesiesInstrument))]
        public List<SharesiesInstrumentXCategory> SharesiesInstrumentXCategoryList { get; set; }


    }
}
