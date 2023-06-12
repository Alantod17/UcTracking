using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Models;
using AwesomeDi.Api.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeDi.Api.Handlers.Auth
{
	public class RefreshTokenParameter
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
	public class RefreshTokenResult
	{
        public string Email { get; set; }
		public string AccessToken { get; set; }
		public DateTime AccessTokenExpireUtcDateTime { get; set; }
		public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpireUtcDateTime { get; set; }
	}
	public class RefreshToken
	{
		private readonly ITokenService _tokenService;
		private readonly _DbContext.AwesomeDiContext _db;

		public RefreshToken(_DbContext.AwesomeDiContext db, ITokenService tokenService)
		{
			_tokenService = tokenService;
			_db = db;
		}
		public List<KeyValuePair<string, string>> Validate(RefreshTokenParameter param)
		{
			var errorList = new List<KeyValuePair<string, string>>();
			string accessToken = param.AccessToken;
			string refreshToken = param.RefreshToken;

			var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
			var username = principal?.Identity?.Name; //this is mapped to the Name claim by default

			var user = _db.User.Include(x=>x.UserTokenList).SingleOrDefault(u => u.Email == username);
			var userToken = user?.UserTokenList.FirstOrDefault(t => t.RefreshToken == refreshToken && t.RefreshTokenExpireUtcDateTime >= DateTime.UtcNow);
			if (user == null || userToken == null)
			{
				errorList.Add(new KeyValuePair<string, string>("Invalid", "Invalid client request"));
			}
			return errorList;
		}

		public RefreshTokenResult Handle(RefreshTokenParameter param)
		{
			string accessToken = param.AccessToken;
			string refreshToken = param.RefreshToken;

			var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
			var username = principal?.Identity?.Name; //this is mapped to the Name claim by default

			var user = _db.User.Include(x=>x.UserTokenList).Single(u => u.Email == username);
			var userToken = user.UserTokenList.First(t => t.RefreshToken == refreshToken && t.RefreshTokenExpireUtcDateTime >= DateTime.UtcNow);

			var newAccessToken = _tokenService.GenerateAccessToken(principal?.Claims);
			var newRefreshToken = _tokenService.GenerateRefreshToken();

			userToken.AccessToken = newAccessToken.accessToken;
			userToken.AccessTokenExpireUtcDateTime = newAccessToken.expireUtcDateTime;
			userToken.RefreshToken = newRefreshToken.refreshToken;
			userToken.RefreshTokenExpireUtcDateTime = newRefreshToken.expireUtcDateTime;
			_db.SaveChanges();
			return new RefreshTokenResult
			{
				Email = user.Email,
				AccessToken = userToken.AccessToken,
				AccessTokenExpireUtcDateTime = (DateTime) userToken.AccessTokenExpireUtcDateTime,
				RefreshTokenExpireUtcDateTime = (DateTime) userToken.RefreshTokenExpireUtcDateTime,
				RefreshToken = userToken.RefreshToken
			};
		}
	}
}
