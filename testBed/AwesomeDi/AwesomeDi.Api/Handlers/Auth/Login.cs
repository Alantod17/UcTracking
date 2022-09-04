using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AwesomeDi.Api.Handlers.Auth.Helper;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;
using AwesomeDi.Api.Service;
using Microsoft.Extensions.Configuration;

namespace AwesomeDi.Api.Handlers.Auth
{
	public class LoginParameter
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string DeviceId { get; set; }
	}
	public class LoginResult
	{
		public string Email { get; set; }
		public string AccessToken { get; set; }
		public DateTime AccessTokenExpireUtcDateTime { get; set; }
		public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpireUtcDateTime { get; set; }
	}
	public class Login
	{
		private readonly _DbContext.AwesomeDiContext _db;
		private readonly ITokenService _tokenService;

		public Login(_DbContext.AwesomeDiContext db, ITokenService tokenService)
		{
			_tokenService = tokenService;
			_db = db;
		}
		public List<KeyValuePair<string, string>> Validate(LoginParameter param)
		{
			var errorList = new List<KeyValuePair<string, string>>();
            var password = HelperString.Encrypt(param.Password);
			var user = _db.User.FirstOrDefault(u => (u.Email.ToLower() == param.Email.ToLower()) && (u.Password == password));
			if (user == null)
			{
				errorList.Add(new KeyValuePair<string, string>("User", "User is invalid"));
			}
			return errorList;
		}

		public LoginResult Handle(LoginParameter param)
		{
            var password = HelperString.Encrypt(param.Password);
            var user = _db.User.First(u => (u.Email.ToLower() == param.Email.ToLower()) && (u.Password == password));

			var userToken = AuthHelper.ProcessUserLogin(_db, _tokenService, param.DeviceId, user);
			return new LoginResult
			{
				Email = user.Email,
				AccessToken = userToken.AccessToken,
				AccessTokenExpireUtcDateTime = userToken.AccessTokenExpireUtcDateTime??DateTime.UtcNow,
				RefreshTokenExpireUtcDateTime = userToken.RefreshTokenExpireUtcDateTime ?? DateTime.UtcNow,
				RefreshToken = userToken.RefreshToken
			};
		}
	}
}
