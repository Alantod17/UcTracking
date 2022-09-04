using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Controllers
{
	[ApiController]
	public class BaseController : ControllerBase
	{
		protected readonly IServiceProvider ServiceProvider;
		public BaseController(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		protected string GetCurrentUserEmail()
		{
			var claimsIdentity = User.Identity as ClaimsIdentity;
			return claimsIdentity?.Claims.FirstOrDefault(x => x.Type == "AwesomeDi_Email")?.Value;
		}
	}
}
