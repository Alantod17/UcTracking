using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AwesomeDi.Api.Handlers.ResearchArticle;
using AwesomeDi.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace AwesomeDi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Researcher, Admin")]
    public class ResearchArticleController : ControllerBase
    {
        readonly _DbContext.AwesomeDiContext _db;

        public ResearchArticleController(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("ImportBibTex")]
        // [AllowAnonymous]
        public ActionResult Get()
        {
            var handler = new ImportDataFromBibTex(_db);
            handler.Handle();
            return Ok();
        }


        [HttpPost]
        [Route("ImportIeee")]
        // [AllowAnonymous]
        public ActionResult ImportIeee()
        {
            var handler = new ImportDataFromIeeeCsv(_db);
            handler.Handle(@"c:\temp\export2021.06.03-05.52.31-relevence.csv");
            return Ok();
        }


        [HttpGet]
        [Route("")]
        public ActionResult Search([FromQuery] SearchResearchArticleParameter param)
        {
            var handler = new SearchResearchArticle(_db);
            var res = handler.Handle(param);
            return Ok(res);
        }
        [HttpGet]
        [Route("Paged")]
        public ActionResult PagedSearch([FromQuery] SearchResearchArticleParameter param)
        {
            var handler = new SearchResearchArticle(_db);
            var res = handler.PagedHandle(param);
            return Ok(res);
        }
        [HttpGet]
        [Route("InfoList")]
        public ActionResult GetResearchArticleInfoList()
        {
	        var handler = new GetResearchArticleInfoList(_db);
	        var res = handler.Handle();
	        return Ok(res);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int? id)
        {
            var handler = new GetResearchArticle(_db);
            var param = new GetResearchArticleParameter { Id = id };
            var errorList = handler.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            var res = handler.Handle(param);
            return Ok(res);
        }

        [HttpPost]
        [Route("{id}")]
        public ActionResult UpdateResearchArticleStatus(int? id, [FromBody] UpdateResearchArticleStatusParameter param)
        {
            var handler = new UpdateResearchArticleStatus(_db);
            param.Id = id;
            var errorList = handler.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            handler.Handle(param);
            return Ok();
        }

        [HttpPost]
        [Route("{id}/ResearchDetail")]
        public ActionResult UpdateResearchArticleResearchDetail(int? id, [FromBody] UpdateResearchArticleResearchDetailParameter param)
        {
            var handler = new UpdateResearchArticleResearchDetail(_db);
            param.Id = id;
            var errorList = handler.Validate(param);
            if (errorList.Any()) return BadRequest(errorList);
            handler.Handle(param);
            return Ok();
        }
    }
}
