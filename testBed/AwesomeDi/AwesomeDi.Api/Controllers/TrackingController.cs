using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AwesomeDi.Api.Handlers.TrackingData;
using AwesomeDi.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace AwesomeDi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackingController : ControllerBase
    {
        
        private IConfiguration _configuration;
        readonly _DbContext.AwesomeDiContext _db;

        public TrackingController(IConfiguration configuration, _DbContext.AwesomeDiContext db)
        {
            _configuration = configuration;
            _db = db;
        }
        

        [HttpPost, AllowAnonymous]
        [Route("UiData")]
        public ActionResult CreteUiData([FromBody]List<CreateTrackingUiDataParameter> param)
        {
            var handler = new CreateTrackingUiData(_db);
            var errorList = handler.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handler.Handle(param);
            return Ok(res);
        }

        [HttpPost, AllowAnonymous]
        [Route("RequestData")]
        public ActionResult CreteRequestData([FromBody] List<CreateTrackingRequestDataParameter> param)
        {
            var handler = new CreateTrackingRequestData(_db);
            var errorList = handler.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handler.Handle(param);
            return Ok(res);
        }
        [HttpPost, AllowAnonymous]
        [Route("EventData")]
        public ActionResult CreteEventData([FromBody] List<CreateTrackingEventDataParameter> param)
        {
            var handler = new CreateTrackingEventData(_db);
            var errorList = handler.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handler.Handle(param);
            return Ok(res);
        }
        [HttpGet]
        [Route("Search")]
        public ActionResult SearchTrackingData([FromQuery] SearchTrackingDataParameter param)
        {
            var handler = new SearchTrackingData(_db);
            var errorList = handler.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handler.Handle(param);
            return Ok(res);
        }
        [HttpGet]
        [Route("Get")]
        public ActionResult GetTrackingData([FromQuery] GetTrackingDataParameter param)
        {
            var handler = new GetTrackingData(_db);
            var errorList = handler.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handler.Handle(param);
            return Ok(res);
        }
    }
}