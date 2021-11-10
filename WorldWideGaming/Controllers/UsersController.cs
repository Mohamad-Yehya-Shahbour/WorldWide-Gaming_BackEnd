using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorldWideGaming.Data;
using WorldWideGaming.Models;
using WorldWideGaming.ViewModels;

namespace WorldWideGaming.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public UsersController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            var userWithSameEmail = _context.Users.Where(u => u.Email == user.Email).SingleOrDefault();// it will return a single element if a match was found or null if its not found
            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email already exist");
            }

            //string uniqueId = Guid.NewGuid().ToString();
            //string destination = Path.Combine("wwwroot", "pictures", uniqueId + ".jpg");
            //using (FileStream fs = new FileStream(destination, FileMode.Create))
            //{
            //    user.fromFile.CopyTo(fs);
            //}
            //string dbUrl = destination.Replace("wwwroot", "").Replace("\\", "/");

            var userObj = new User
            {
                UserName=user.UserName,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                //ImageUrl = dbUrl,
                // add all  other properties from your model and give it values
            };
            _context.Users.Add(userObj);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);

        }


        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var userEmail = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userEmail == null)// if there is no such email
            {
                return NotFound("There is no such Email!");
            }

            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password)) // we have used the useremail to access the paremeters in the database table users
            {
                return Unauthorized("Your Password is Wrong !");
            } // if the password and the hashed password in the database is not the same return unauthorized

            var claims = new[]
             {
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(ClaimTypes.Email, user.Email),
               //new Claim(ClaimTypes.Role,userEmail.Role)
             };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                User_id = userEmail.Id,
                User_name = userEmail.UserName
            }); // this return will show the access_token and many other things  if you want you can delete the rest and just keep the access token and you can add stuff like id for example
        }


        [HttpGet]
        [Authorize]
        public async Task<User> userProfile([FromQuery] int userId)
        {
            User user = await _context.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();

            return user;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<User>> getUserFriends([FromQuery] int userId)
        {
            List<Friend> friend1 = await _context.Friends.Where(x => x.UserOneId == userId).Where(x => x.IsPending == 0).ToListAsync();
            List<Friend> friend2 = await _context.Friends.Where(x => x.UserTwoId == userId).Where(x => x.IsPending == 0).ToListAsync();
            List<Friend> friends = new List<Friend>();

            List<int> list = new List<int>();

            if (friend1.Count() == 0)
            {
                friends = friend2;
            }
            else if (friend2.Count() == 0)
            {
                friends = friend1;
            }
            else
            {
                friends = friend1.Union(friend2).ToList();
            }
 
            foreach (var friend in friends)
            {
                list.Add(friend.UserOneId);
                list.Add(friend.UserTwoId);
            }
            list = list.Distinct().ToList();
            list.RemoveAll(x => x == userId);

            List<User> users = _context.Users.Where(p => list.Contains(p.Id)).ToList();
            return users;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<User>> getAllusers([FromQuery] int userId)
        {
            return await _context.Users.Where(x => x.Id != userId).ToListAsync();
        }

        [HttpGet]
        [Authorize]
        public async Task<List<User>> getUsersByName([FromQuery] string name, [FromQuery] int userId)
        {
            return await _context.Users.Where(x => x.UserName.Contains(name)).Where(x => x.Id != userId).ToListAsync();
        }

        [HttpGet]
        [Authorize]
        public IQueryable getCountries()
        {
            var query = _context.Users
                   .GroupBy(p => p.Country)
                   .Select(g => new { Country = g.Key, count = g.Count() });
  
            return query;
        }
        //colling external apis / get
        [HttpGet]
        public async Task<IActionResult> testGet()
        {
            var client = new RestClient("https://jsonplaceholder.typicode.com/");
            IRestRequest restRequest = new RestRequest("todos/1", Method.GET, DataFormat.Json);
            var response = await client.ExecuteAsync(restRequest);

            return Ok(response.Content);
        }


        // colling external apis / post
        [HttpPost]
        public async Task<IActionResult> testPost()
        {
            var client = new RestClient("https://jsonplaceholder.typicode.com/");

            var postData = new
            {
                title = "foo",
                body = "bar",
                userId = 1
            };

            IRestRequest restRequest = new RestRequest("posts", Method.POST, DataFormat.Json)
                .AddJsonBody(postData);

            var response = await client.ExecuteAsync(restRequest);

            return Ok(response.Content);
        }








    }
}
