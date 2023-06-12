using System;
using System.Linq;
using System.Threading.Tasks;
using AwesomeDi.Api.Handlers.Auth;
using AwesomeDi.Api.Models;
using AwesomeDi.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeDi.Api.Controllers
{
	[Route("auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		readonly _DbContext.AwesomeDiContext _db;
		readonly ITokenService _tokenService;
		public AuthController(_DbContext.AwesomeDiContext db, ITokenService tokenService)
		{
			_db = db ?? throw new ArgumentNullException(nameof(_db));
			_tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
		}

		[HttpPost, Route("login"), AllowAnonymous]
		public IActionResult Login([FromBody] LoginParameter param)
		{
			var handle = new Login(_db,_tokenService);
			var errorList = handle.Validate(param);
			if (errorList.Any()) return BadRequest(errorList);
			var res = handle.Handle(param);
			return Ok(res);
		}


		[HttpPost, Route("refreshToken")]
		public IActionResult RefreshToken([FromBody]RefreshTokenParameter param)
		{
			var handle = new RefreshToken(_db, _tokenService);
			var errorList = handle.Validate(param);
			if (errorList.Any()) return BadRequest(errorList);
			var res = handle.Handle(param);
			return Ok(res);
		}



		[HttpPost, Authorize(Roles = "Admin")]
		[Route("revokeToken")]
		public IActionResult Revoke([FromBody] RevokeTokenParameter param)
		{
			var handle = new RevokeToken(_db);
			var errorList = handle.Validate(param, User);
			if (errorList.Any()) return BadRequest(errorList);
			handle.Handle(param, User);
			return Ok();
		}


        [HttpPost, Route("GoogleLogin"), AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginParameter param)
        {
            var handle = new GoogleLogin(_db, _tokenService);
            var errorList = await handle.ValidateAsync(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handle.Handle(param);
            return Ok(res);
        }
	}
}
