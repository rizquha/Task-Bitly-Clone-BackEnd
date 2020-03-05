using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Task_Bitly_Clone.Models;

namespace Task_Bitly_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ILogger<TrackController> _logger;
        private AppDBContext _AppDbContext;
        public IConfiguration Configuration;
        public TrackController(ILogger<TrackController> logger, AppDBContext appDbContext, IConfiguration configuration)
        {
            _logger = logger;
            _AppDbContext = appDbContext;
            Configuration = configuration;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var token = Request.Headers["Authorization"];
            token = token.ToString().Substring(7);
            var jwtSecrTokenHandler = new JwtSecurityTokenHandler();
            var secrToken = jwtSecrTokenHandler.ReadToken(token) as JwtSecurityToken;
            var username = secrToken?.Claims.First(claim => claim.Type == "sub").Value;
            int idUserLogin = 0;
            var id = (from i in _AppDbContext.users where i.Username == username select i).First();
            idUserLogin = id.Id;
            var url = from i in _AppDbContext.shortUrl where i.UserId==idUserLogin select i;

            return Ok(url);
        }

        [HttpGet("{url}")]
        public IActionResult URL(string url)
        {
            var _url = (from i in _AppDbContext.shortUrl where i.ShortUrl == url select i).First();
            var urls = from i in _AppDbContext.tracks where i.ShortUrlId == _url.Id select i;

            var record = new UrlStatisticDataView(urls.ToList());

            var recordToSend = new
            {
                Click = record.Clicked,
                byDate = record.ByDate,
                byMonth = record.ByMonth,
                byYear = record.ByYear
            };

            return Ok(recordToSend);
        }

        [HttpGet("track/{url}")]
        public IActionResult Track(string url)
        {
            return Ok(url);
        }
    }
}
