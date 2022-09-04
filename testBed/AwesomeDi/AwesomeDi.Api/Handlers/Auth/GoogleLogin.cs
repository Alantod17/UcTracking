using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Handlers.Auth.Helper;
using AwesomeDi.Api.Models;
using AwesomeDi.Api.Service;
using Google.Apis.Auth;

namespace AwesomeDi.Api.Handlers.Auth
{
	public class GoogleLoginParameter
	{
		public string Email { get; set; }
		public string IdToken { get; set; }
		public string DeviceId { get; set; }
	}
	public class GoogleLoginResult
	{
		public string Email { get; set; }
		public string AccessToken { get; set; }
		public DateTime AccessTokenExpireUtcDateTime { get; set; }
		public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpireUtcDateTime { get; set; }
	}
	public class GoogleLogin
	{
		private readonly _DbContext.AwesomeDiContext _db;
		private readonly ITokenService _tokenService;

		public GoogleLogin(_DbContext.AwesomeDiContext db, ITokenService tokenService)
		{
			_tokenService = tokenService;
			_db = db;
		}
		public async System.Threading.Tasks.Task<List<KeyValuePair<string, string>>> ValidateAsync(GoogleLoginParameter param)
		{
			var errorList = new List<KeyValuePair<string, string>>();
			var user = _db.User.FirstOrDefault(u => (u.Email == param.Email));
			if (user == null)
			{
				errorList.Add(new KeyValuePair<string, string>("User", "User is invalid"));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(param.IdToken))
                {
				    errorList.Add(new KeyValuePair<string, string>("IdToken", "Id token is invalid"));
                }
                else
                {
                    try
                    {
                        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(param.IdToken);
                        if (payload.Email.ToLower() != user.Email.ToLower() || !payload.EmailVerified)
                        {
				            errorList.Add(new KeyValuePair<string, string>("IdToken", "Id token is invalid or email is not verified"));
                        }
                    }
                    catch (Exception)
                    {
				        errorList.Add(new KeyValuePair<string, string>("IdToken", "Id token is invalid"));
                    }
				}
			}

			return errorList;
		}

		public GoogleLoginResult Handle(GoogleLoginParameter param)
		{
			var user = _db.User.First(u => (u.Email == param.Email) );
			var userToken = AuthHelper.ProcessUserLogin(_db, _tokenService, param.DeviceId, user);
            return new GoogleLoginResult
			{
				Email = user.Email,
				AccessToken = userToken.AccessToken,
				AccessTokenExpireUtcDateTime = userToken.AccessTokenExpireUtcDateTime??DateTime.UtcNow,
				RefreshTokenExpireUtcDateTime = userToken.RefreshTokenExpireUtcDateTime??DateTime.UtcNow,
				RefreshToken = userToken.RefreshToken
			};
		}

    }
}
