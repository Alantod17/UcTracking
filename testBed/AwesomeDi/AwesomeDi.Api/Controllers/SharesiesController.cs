using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AwesomeDi.Api.Handlers.Sharesies;
using AwesomeDi.Api.Models;
using AwesomeDi.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AwesomeDi.Api.Controllers
{
	[ApiController]
	public class SharesiesController : ControllerBase
	{


        readonly _DbContext.AwesomeDiContext _db;
        readonly ISharesiesService _sharesiesService;
        readonly ILogger<SharesiesController> _logger;

        public SharesiesController(_DbContext.AwesomeDiContext db, ISharesiesService sharesiesService, ILogger<SharesiesController> logger)
        {
            _db = db;
            _sharesiesService = sharesiesService;
            _logger = logger;
        }

		[AllowAnonymous]
		[Route("GetSharesisesData")]
		public async Task<IActionResult> GetSharesisesData()
        {
            var handler = new GetSharesisesData(_db, _sharesiesService, _logger);
            await handler.Handle();
            return Ok();
        }

        [HttpGet]
        [Route("SharesiesInstruments")]
        public ActionResult SearchSharesiesInstrument([FromQuery] SearchSharesiesInstrumentParameter param)
        {
            var handler = new SearchSharesiesInstrument(_db);
            var res = handler.PagedHandle(param);
            return Ok(res);
        }
    }
}
