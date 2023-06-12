using System.ComponentModel.DataAnnotations;

namespace AwesomeDi.Api.Models
{
	public class SharesiesConfiguration : DatedEntity
	{
		public SharesiesConfiguration()
		{
		}
		[Key] public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
    }
}
