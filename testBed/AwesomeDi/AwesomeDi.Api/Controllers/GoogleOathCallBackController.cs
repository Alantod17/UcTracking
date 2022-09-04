using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;

namespace AwesomeDi.Api.Controllers
{
	[ApiController]
	public class GoogleOathCallBackController : ControllerBase
	{

		// [HttpGet]
		// // [Route("google-response")]
		// [AllowAnonymous]
		// [Route("signin-google")]
		//
		// public async Task GoogleOathCallBackAsync()
		// {
		// 	var con = HttpContext;
		// 	// var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		//
		// }
		//
		//
		// [AllowAnonymous]
		// [Route("google-login")]
		// public IActionResult GoogleLogin()
		// {
		// 	var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleOathCallBack") };
		// 	return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		// }
	}
}
