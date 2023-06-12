using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeDi.Api.Models
{
	public class SharesiesInstrumentCategory : DatedEntity
	{
		[Key] public int Id { get; set; }
        public string Name { get; set; }
        [InverseProperty(nameof(SharesiesInstrumentXCategory.SharesiesInstrumentCategory))]
        public List<SharesiesInstrumentXCategory> SharesiesInstrumentXCategoryList { get; set; }
    }
}
