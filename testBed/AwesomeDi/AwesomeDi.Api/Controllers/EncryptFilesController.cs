using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using AwesomeDi.Api.Handlers.FileEntry;
using Microsoft.Extensions.Logging;

namespace AwesomeDi.Api.Controllers
{
	[ApiController]
	public class EncryptFilesController: ControllerBase
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<EncryptFilesController> _logger;

		public EncryptFilesController(IServiceProvider serviceProvider, ILogger<EncryptFilesController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
		[HttpGet]
		[HttpPost]
		[Route("EncryptLocalFiles")]
		public ActionResult EncryptLocalFiles()
		{
			var param = new EncryptFilesParameter();
			param.PathToEncrypt = @"F:\Photo";
			param.OutputPath = @"G:\OneDriveN62\OneDrive - for personal";
			// param.PathToEncrypt = @"F:\Software Backup";
			// param.OutputPath = @"G:\Test";
			var handle = new EncryptFiles(_serviceProvider, _logger);
			var errorList = handle.Validate(param);
			if (errorList.Any()) return BadRequest(errorList);
			handle.Handle(param);
			return Ok();
		}

	}
}
