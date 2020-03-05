using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Task_Bitly_Clone.Models;

namespace Task_Bitly_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private AppDBContext _AppDbContext;
        public IConfiguration Configuration;
        private static List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        private static List<char> characters = new List<char>() {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
                                                        'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
                                                        'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
                                                        'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
        public HomeController(ILogger<HomeController> logger, AppDBContext appDbContext, IConfiguration configuration)
        {
            _logger = logger;
            _AppDbContext = appDbContext;
            Configuration = configuration;
        }

      
         // GET: api/Home/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Home
        [HttpPost("Register")]
        public IActionResult Register(Users users)
        {
            var user = new Users
            {
                Username = users.Username,
                Email = users.Email,
                Password= users.Password,
            };
            _AppDbContext.Add(user);
            _AppDbContext.SaveChanges();
            return Ok("berhasil regis");
        }

        [HttpPost("Login")]
        public IActionResult Login(Users users)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticatedUser (users.Username, users.Password);
            if (user != null)
            {
                var token = GenerateJwtToken(user);
                HttpContext.Session.SetString("JWTToken", token);
                var get = HttpContext.Session.GetString("JWTToken");
                Console.WriteLine(token);
                var userList = from i in _AppDbContext.users select i;
                foreach (var i in userList)
                {
                    if (i.Username == users.Username && i.Password == users.Password)
                    {
                        HttpContext.Response.Cookies.Append("Username", users.Username);
                        return Ok(new { tokenuser = token });
                    }
                }
            }
            return Ok("Berhasil");
        }

        private string GenerateJwtToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim (JwtRegisteredClaimNames.Sub, Convert.ToString (user.Username)),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ())
            };

            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(2000),
                signingCredentials: credentials);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        private Users AuthenticatedUser(string username, string password)
        {
            Users user = null;
            var get = from i in _AppDbContext.users select i;
            foreach (var i in get)
            {
                if (i.Username == username && i.Password == password)
                {
                    user = new Users
                    {
                        Username = username,
                        Password = password,
                    };
                }
            }
            return user;
        }
        [HttpPost("ShortURLNotLogin")]
        public IActionResult ShortURLNotLogin(ShortUrls url)
        {
            string URL = "";
            Random rand = new Random();
            for (int i = 0; i < 11; i++)
            {
                int random = rand.Next(0, 3);
                if (random == 1)
                {
                    random = rand.Next(0, numbers.Count);
                    URL += numbers[random].ToString();
                }
                else
                {
                    random = rand.Next(0, characters.Count);
                    URL += characters[random].ToString();
                }
            }
            var x = new ShortUrls
            {
                Title = url.Title,
                ShortUrl = URL,
                Url = url.Url,
                UserId = 0,
                CreatedAt = DateTime.Now
            };
            _AppDbContext.Add(x);
            _AppDbContext.SaveChanges();
            return Ok(new { shorturl = URL });
        }
        [Authorize]
        [HttpPost("ShortURL")]
        public IActionResult ShortURL(ShortUrls url)
        {
            var token = Request.Headers["Authorization"];
            token = token.ToString().Substring(7);
            var jwtSecrTokenHandler = new JwtSecurityTokenHandler();
            var secrToken = jwtSecrTokenHandler.ReadToken(token) as JwtSecurityToken;
            var username = secrToken?.Claims.First(claim => claim.Type == "sub").Value;
             Console.WriteLine(token);
            Console.WriteLine(username);
            Console.WriteLine(Request.Cookies["username"]);
            Console.WriteLine("COBA");
            Console.WriteLine(url.ShortUrl);
            Console.WriteLine("COBA");


            var cookie = "";
            if (Request.Cookies["Username"] != null)
            {
                cookie = Request.Cookies["Username"];
            }
            var id = from i in _AppDbContext.users where i.Username == username select i;
            int idUserLogin = 0;
            foreach (var i in id)
            {
                idUserLogin = i.Id;
            }
            if (url.ShortUrl == "")
            {
                string URL = "";
                Random rand = new Random();
                for (int i = 0; i < 11; i++)
                {
                    int random = rand.Next(0, 3);
                    if (random == 1)
                    {
                        random = rand.Next(0, numbers.Count);
                        URL += numbers[random].ToString();
                    }
                    else
                    {
                        random = rand.Next(0, characters.Count);
                        URL += characters[random].ToString();
                    }
                }
                var x = new ShortUrls
                {
                    Title = url.Title,
                    ShortUrl = URL,
                    Url = url.Url,
                    UserId =idUserLogin,
                    CreatedAt = DateTime.Now
                };
                _AppDbContext.Add(x);
                _AppDbContext.SaveChanges();
                return Ok(new { shorturl = URL });

            }
            else
            {
                string URL = url.ShortUrl ;
                var x = new ShortUrls
                {
                    Title = url.Title,
                    ShortUrl = url.ShortUrl,
                    Url = url.Url,
                    UserId = idUserLogin,
                    CreatedAt = DateTime.Now
                };
                _AppDbContext.Add(x);
                _AppDbContext.SaveChanges();
                return Ok(new { shorturl = url.ShortUrl });
            }
        }
    }
}
