using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AwesomeDi.Api.Models;
using AwesomeDi.Api.Service;

namespace AwesomeDi.Api.Handlers.Auth.Helper
{
    public static class AuthHelper
    {

        public static UserToken ProcessUserLogin(_DbContext.AwesomeDiContext db, ITokenService tokenService,string deviceId, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
            if(!string.IsNullOrWhiteSpace(user.Role))
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            var accessTokenInfo = tokenService.GenerateAccessToken(claims);
            var refreshTokenInfo = tokenService.GenerateRefreshToken();
            var userToken = user.UserTokenList.FirstOrDefault(x => x.DeviceId == deviceId);
            if (userToken == null)
            {
                userToken = new UserToken();
                userToken.User = user;
                userToken.DeviceId = deviceId;
                db.UserToken.Add(userToken);
            }

            userToken.AccessToken = accessTokenInfo.accessToken;
            userToken.AccessTokenExpireUtcDateTime = accessTokenInfo.expireUtcDateTime;
            userToken.RefreshToken = refreshTokenInfo.refreshToken;
            userToken.RefreshTokenExpireUtcDateTime = refreshTokenInfo.expireUtcDateTime;
            db.SaveChanges();
            return userToken;
        }
    }
}
