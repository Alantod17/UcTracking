using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AwesomeDi.Api.Handlers.FileEntry;
using AwesomeDi.Api.Models;
using AwesomeDi.Api.Service;
using Microsoft.AspNetCore.Authorization;

namespace AwesomeDi.Api.Controllers
{
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class FileEntryController : ControllerBase
	{
		
		readonly _DbContext.AwesomeDiContext _db;
		readonly ITokenService _tokenService;

		public FileEntryController(_DbContext.AwesomeDiContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

		[HttpGet]
		[Route("file")]
		[Route("api/file")]
		public ActionResult Get([FromQuery] SearchFileEntryParameter param)
		{
			var handle = new SearchFileEntry(_db);
            var errorList = handle.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            return Ok(handle.Handle(param));
        }

        [HttpGet]
        [Route("fileGroup")]
        [Route("api/fileGroup")]
        public ActionResult Get([FromQuery] SearchFileEntryGroupParameter param)
        {
            var handle = new SearchFileEntryGroup(_db);
            var errorList = handle.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            return Ok(handle.Handle(param));
        }

        [HttpGet, AllowAnonymous]
        [Route("file/{id}/token")]
        public ActionResult Get(int id, [FromQuery]string token)
        {
            if (!_tokenService.IsValidToken(token)) return Unauthorized();
            var handle = new GetFileEntry(_db);
            var param = new GetFileEntryParameter {Id = id};
            var errorList = handle.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handle.Handle(param);
            return PhysicalFile(res.FilePath, res.FileMimeType, enableRangeProcessing: true);
        }

        [HttpGet]
        [Route("file/{id}")]
        public ActionResult Get(int id)
        {
            var handle = new GetFileEntry(_db);
            var param = new GetFileEntryParameter { Id = id };
            var errorList = handle.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handle.Handle(param);
            return PhysicalFile(res.FilePath, res.FileMimeType, enableRangeProcessing: true);
        }

        [HttpGet]
        [Route("file/{id}/thumbnail")]
        public ActionResult GetThumbnail(int id)
        {
            var handle = new GetThumbnailImageV2(_db);
            var param = new GetThumbnailImageV2Parameter { Id = id };
            var errorList = handle.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handle.Handle(param);
            return PhysicalFile(res.ThumbnailFilePath, "image/png", enableRangeProcessing: true);
        }


        [HttpGet,AllowAnonymous]
        [Route("file/{id}/thumbnail/token")]
        public ActionResult GetThumbnail(int id, [FromQuery] string token, [FromQuery] bool fullSizeThumb = false)
        {
            if (!_tokenService.IsValidToken(token)) return Unauthorized();
            var handle = new GetThumbnailImageV2(_db);
            var param = new GetThumbnailImageV2Parameter { Id = id, IsFullSizeThumbnail = fullSizeThumb};
            var errorList = handle.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handle.Handle(param);
            return PhysicalFile(res.ThumbnailFilePath, "image/png", enableRangeProcessing: true);
        }

        [HttpGet]
        [Route("file/Get")]
        public ActionResult Get([FromQuery] GetFileEntryParameter param)
        {
            var handle = new GetFileEntry(_db);
            var errorList = handle.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            return Ok(handle.Handle(param));
        }
		[HttpGet]
        [Route("file/GetThumbnailImage")]
        public ActionResult Get([FromQuery] GetThumbnailImageParameter param)
        {
            var handle = new GetThumbnailImage(_db);
            var errorList = handle.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            return Ok(handle.Handle(param));
        }
	}
}




// [HttpGet, AllowAnonymous]
// [Route("file/{id}/thumbnail")]
// public ActionResult GetThumbnail(int id, [FromQuery] string token)
// {
//     if (!_tokenService.IsValidToken(token)) return Unauthorized();
//     var handle = new GetThumbnailImageV2(_db);
//     var param = new GetThumbnailImageV2Parameter { Id = id };
//     var errorList = handle.Validate(param);
//     if (errorList.Any()) return BadRequest(errorList);
//     var res = handle.Handle(param);
//     return PhysicalFile(res.ThumbnailFilePath, "image/png", enableRangeProcessing: true);
// }