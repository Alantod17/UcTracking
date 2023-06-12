using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.Auth
{
	public class RevokeTokenParameter
	{
		public string DeviceId { get; set; }
	}
	public class RevokeToken
	{
		private readonly _DbContext.AwesomeDiContext _db;

		public RevokeToken(_DbContext.AwesomeDiContext db)
		{
			_db = db;
		}
		public List<KeyValuePair<string, string>> Validate(RevokeTokenParameter param, ClaimsPrincipal principal)
		{
			var errorList = new List<KeyValuePair<string, string>>();
			var username = principal?.Identity?.Name;
			var user = _db.User.FirstOrDefault(u => u.Email == username);
			if (user == null)
			{
				errorList.Add(new KeyValuePair<string, string>("User", "User is invalid"));
			}
			return errorList;
		}

		public void Handle(RevokeTokenParameter param, ClaimsPrincipal principal)
		{
			var username = principal?.Identity?.Name;
			if (username != null)
			{
				var tokenList = _db.UserToken.Where(x => x.User.Email == username && x.DeviceId == param.DeviceId)
					.ToList();
				_db.UserToken.RemoveRange(tokenList);
				_db.SaveChanges();
			}
		}
	}
}
