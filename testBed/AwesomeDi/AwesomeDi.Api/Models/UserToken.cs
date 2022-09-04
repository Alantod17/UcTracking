using System;
using System.ComponentModel.DataAnnotations;

namespace AwesomeDi.Api.Models
{
	public class UserToken : DatedEntity
	{
		[Key] public int Id { get; set; }
		[Required] public User User { get; set; }
		public string AccessToken { get; set; }
		public DateTime? AccessTokenExpireUtcDateTime { get; set; }
		public string RefreshToken { get; set; }
		public DateTime? RefreshTokenExpireUtcDateTime { get; set; }
		public string DeviceId { get; set; }
	}
}
