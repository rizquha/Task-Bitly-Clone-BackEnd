using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Task_Bitly_Clone.Models;

namespace Task_Bitly_Clone.Controllers
{
    [Route("")]
    [ApiController]
    public class GetUrlController : ControllerBase
    {
        private readonly ILogger<GetUrlController> _logger;
        private AppDBContext _AppDbContext;
        public IConfiguration Configuration;
       public GetUrlController(ILogger<GetUrlController> logger, AppDBContext appDbContext, IConfiguration configuration)
        {
            _logger = logger;
            _AppDbContext = appDbContext;
            Configuration = configuration;
        }

        [HttpGet("{url}")]
        public IActionResult Get(string url)
        {
            var IP = Request.HttpContext.Connection.RemoteIpAddress;
            string referrerURL = "";
            int id = 0;
            
            var z = from i in _AppDbContext.shortUrl where i.ShortUrl == url select i;
            var x = z.First();
            if (x != null)
            {
                    referrerURL = x.Url;
                    id = x.Id;
                    var y = new Tracks
                    {
                        ShortUrlId = id,
                        IpAddress = IP.ToString(),
                        ReferrerUrl = referrerURL,
                        CreatedAt = DateTime.Now
                    };
                    _AppDbContext.Add(y);
                    _AppDbContext.SaveChanges();
                    return Redirect(x.Url);
            }
            return Ok("Link non active");
        }

    }
}
