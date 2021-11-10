using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WorldWideGaming.Data;
using WorldWideGaming.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldWideGaming.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public EventsController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> createEvent([FromForm] Event eventObj)
        {
            if (eventObj != null)
            {
                string uniqueId = Guid.NewGuid().ToString();
                string destination = Path.Combine("wwwroot", "pictures", uniqueId + ".jpg");
                using (FileStream fs = new FileStream(destination, FileMode.Create))
                {
                    eventObj.formFile.CopyTo(fs);
                }
                string dbUrl = destination.Replace("wwwroot", "").Replace("\\", "/");

                var eventToAdd = new Event()
                {
                    Title = eventObj.Title,
                    Description = eventObj.Description,
                    Long = eventObj.Long,
                    Lat = eventObj.Lat,
                    ImageUrl = dbUrl,
                    JoinersNum = 1,
                    UserId = eventObj.UserId
                };
                await _context.Events.AddAsync(eventToAdd);
                await _context.SaveChangesAsync();

                List<Friend> friend1 = await _context.Friends.Where(x => x.UserOneId == eventObj.UserId).Where(x => x.IsPending == 0).ToListAsync();
                List<Friend> friend2 = await _context.Friends.Where(x => x.UserTwoId == eventObj.UserId).Where(x => x.IsPending == 0).ToListAsync();
                List<Friend> friends = new List<Friend>();
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
                if (friends.Count() == 0)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }

                List<int> list = new List<int>();

                foreach (var friend in friends)
                {
                    list.Add(friend.UserOneId);
                    list.Add(friend.UserTwoId);
                }
                list = list.Distinct().ToList();
                 for (var i = 0; i < list.Count; i++)
                {
                    var notification = new Notification()
                    {
                        User1Id = eventObj.UserId,
                        User2Id = list[i],
                    };
                    await _context.Notifications.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }

                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Event>> getEvents()
        {
            return await _context.Events.ToListAsync();
            
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Event>> getUserEvents([FromQuery] int userId)
        {
            List<EventsJoiner> eventsJoiners = await _context.EventsJoiners.Where(x => x.UserId == userId).ToListAsync();

            List<int> list = new List<int>();

            foreach (var eventsJoiner in eventsJoiners)
            {
                list.Add(eventsJoiner.EventId);
            }

            List<Event> events = await _context.Events.Where(p => list.Contains(p.Id)).Include(x => x.User).ToListAsync();
            return events;

        }

        [HttpGet]
        [Authorize]
        public async Task<List<Event>> getUserEventsById([FromQuery] int userId)
        {
            return await _context.Events.Where(x => x.UserId == userId).Include(x => x.User).ToListAsync();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> joinEvent([FromBody] EventsJoiner eventsJoiner)
        {
            var check = await _context.EventsJoiners.Where(x => x.EventId == eventsJoiner.EventId).Where(x => x.UserId == eventsJoiner.UserId).SingleOrDefaultAsync();
            if (check == null)
            {
                var eventJoiner = new EventsJoiner()
                {
                    UserId = eventsJoiner.UserId,
                    EventId = eventsJoiner.EventId,
                };
                await _context.EventsJoiners.AddAsync(eventJoiner);
                await _context.SaveChangesAsync();

                var eventObj = await _context.Events.SingleOrDefaultAsync(x => x.Id == eventsJoiner.EventId);
                var joiners = eventObj.JoinersNum;
                eventObj.JoinersNum = joiners + 1;
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Notification>> getNotif([FromQuery] int userId)
        {
            return await _context.Notifications.Where(x => x.User2Id == userId).Include(x => x.User1).ToListAsync();
        }


    }
}
