using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeDi.Api.Handlers.Sharesies;
using AwesomeDi.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace AwesomeDi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private IConfiguration _configuration;
        readonly _DbContext.AwesomeDiContext _db;

        public InfoController(ILogger<WeatherForecastController> logger, IConfiguration configuration, _DbContext.AwesomeDiContext db)
        {
            _logger = logger;
            _configuration = configuration;
            _db = db;
        }

        public class Result
        {
            public string DockerRegistryServerUsername { get; set; }
            public string AppinsightsInstrumentationkey { get; set; }
            public string Version { get; set; }
            public int SharesiesInstrumentCount { get; set; }
            public string HostName { get; set; }
            public string TestVersion { get; set; }
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Get()
        {
            var res = new Result();
            res.Version = _configuration.GetSection("AwesomeDiConfig")["VersionNumber"];
            res.HostName = _configuration["HOSTNAME"];
            res.DockerRegistryServerUsername = _configuration["DOCKER_REGISTRY_SERVER_USERNAME"]??"No Value set";
            res.AppinsightsInstrumentationkey = _configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]??"No Value";
            res.TestVersion = _configuration["TestVersion"] ??"No Value";
            
            var handler = new SearchSharesiesInstrument(_db);
            res.SharesiesInstrumentCount = handler.PagedHandle(new SearchSharesiesInstrumentParameter()).TotalCount;
            return Ok(res);
        }
    }
}