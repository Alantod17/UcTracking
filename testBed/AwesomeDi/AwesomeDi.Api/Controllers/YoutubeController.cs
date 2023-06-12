using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwesomeDi.Api.Handlers.Youtube;
using Microsoft.AspNetCore.Authorization;

namespace AwesomeDi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YoutubeController : ControllerBase
    {
        [HttpPost]
        [Route("DownloadPlayList")]
        [AllowAnonymous]
        public async Task<ActionResult> DownloadPlayList([FromBody]DownloadPlayList.Parameter param)
        {
            var handler = new DownloadPlayList();
            await handler.HandleAsync(param);
            return Ok();
        }
    }
}
