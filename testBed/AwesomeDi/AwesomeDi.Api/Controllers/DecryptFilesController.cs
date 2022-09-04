using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AwesomeDi.Api.Handlers;
using AwesomeDi.Api.Handlers.FileEntry;

namespace AwesomeDi.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class DecryptFilesController : ControllerBase
	{
		[HttpPost]
		public ActionResult EncryptFiles([FromBody] DecryptFilesParameter param)
		{
			var handle = new DecryptFiles();
			var errorList = handle.Validate(param);
			if (errorList.Any()) return BadRequest(errorList);
			handle.Handle(param);
			return Ok();
		}
	}
}
