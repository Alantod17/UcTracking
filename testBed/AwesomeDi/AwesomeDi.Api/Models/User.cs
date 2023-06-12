using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeDi.Api.Models
{
	public class User : DatedEntity
	{
		public User()
		{
			UserTokenList = new List<UserToken>();
		}
		[Key] public int Id { get; set; }
		[Required] public string Password { get; set; }
		[Required] public string Email { get; set; }
		[Required] public bool Active { get; set; }
		public string Role { get; set; }
		[InverseProperty("User")]
		public List<UserToken> UserTokenList { get; set; }
	}
}
