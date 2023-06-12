using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AwesomeDi.Api.Service
{
	public interface ITokenService
	{
		(string accessToken, DateTime expireUtcDateTime) GenerateAccessToken(IEnumerable<Claim> claims);
		(string refreshToken, DateTime expireUtcDateTime) GenerateRefreshToken();
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
		bool IsValidToken(string token);
	}
    public class TokenService : ITokenService
    {
	    private readonly string _symmetricSecurityKey;
	    private readonly string _issuer;
	    private readonly string _audience;
	    private readonly int _accessTokenLifeTimeMins;
	    private readonly int _refreshTokenLifeTimeDays;

	    public TokenService(IConfiguration configuration)
        {
	        _symmetricSecurityKey = configuration.GetSection("AuthConfig")["SymmetricSecurityKey"];
            _issuer = configuration.GetSection("AuthConfig")["Issuer"];
            _audience = configuration.GetSection("AuthConfig")["Audience"];
            _accessTokenLifeTimeMins = int.Parse(configuration.GetSection("AuthConfig")["AccessTokenLifeTimeMins"]);
            _refreshTokenLifeTimeDays = int.Parse(configuration.GetSection("AuthConfig")["RefreshTokenLifeTimeDays"]);
        }
	    
	    public (string accessToken, DateTime expireUtcDateTime) GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_symmetricSecurityKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expireUtcDateTime = DateTime.UtcNow.AddMinutes(_accessTokenLifeTimeMins);
            var tokeOptions = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expireUtcDateTime,
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return (tokenString, expireUtcDateTime);
        }

        public (string refreshToken, DateTime expireUtcDateTime) GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return (Convert.ToBase64String(randomNumber), DateTime.UtcNow.AddDays(_refreshTokenLifeTimeDays));
            }
        }

        public bool IsValidToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, 
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_symmetricSecurityKey)),
                ValidateLifetime = true
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_symmetricSecurityKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}
