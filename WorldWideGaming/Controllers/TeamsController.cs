using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldWideGaming.Data;
using WorldWideGaming.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldWideGaming.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public TeamsController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Team>> getTeams()
        {
            List<Team> teams = await _context.Teams.ToListAsync();
            return teams;
        }

        [HttpPost]
        [Authorize]
        public async Task<Team> getTeamById([FromForm]int teamId)
        {
            return await _context.Teams.SingleOrDefaultAsync(x => x.Id == teamId);
        }

        [HttpPost]
        [Authorize]
        public async Task<List<TeamsAchievement>> getTeamAchievements([FromForm] int teamId)
        {
            List<TeamsAchievement> teamsAchievements = await _context.TeamsAchievements.Where(x => x.TeamId == teamId).ToListAsync();
            return teamsAchievements;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> addTeamAchievement([FromForm] TeamsAchievement teamsAchievement)
        {
            var achievement = new TeamsAchievement()
            {
                Body = teamsAchievement.Body,
                TeamId = teamsAchievement.TeamId,
            };
            if(achievement == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                await _context.TeamsAchievements.AddAsync(achievement);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            
        }

        [HttpPost]
        [Authorize]
        public async Task<List<User>> getTeamMembers([FromForm] int teamId)
        {
            List<UsersTeam> usersTeams = await _context.UsersTeams.Where(x => x.TeamId == teamId).ToListAsync();
            List<int> list = new List<int>();
            foreach (var usersTeam in usersTeams)
            {
                var temp = usersTeam.UserId;
                list.Add(temp);
            }

            List<User> users = await _context.Users.Where(p => list.Contains(p.Id)).ToListAsync();
            return users;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> createTeam([FromForm] Team team)
        {
            var teamToAdd = new Team()
            { 
                Name = team.Name,
                Description = team.Description,
                AdminId = team.AdminId,
            };
            if(teamToAdd == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                await _context.Teams.AddAsync(teamToAdd);
                await _context.SaveChangesAsync();

                Team userTeam = await _context.Teams.SingleOrDefaultAsync(x => x.AdminId == team.AdminId);

                var teamId = userTeam.Id;
                var adminId = userTeam.AdminId;

                var usersTeams = new UsersTeam()
                {
                    UserId = adminId,
                    TeamId = adminId,
                    IsPending = false,
                };
                if(usersTeams == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    await _context.UsersTeams.AddAsync(usersTeams);
                    await _context.SaveChangesAsync();

                    User user = await _context.Users.SingleOrDefaultAsync(x => x.Id == team.AdminId);
                    user.IsTeamAdmin = true;
                    user.IsInTeam = true;
                    await _context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status201Created);
                }
                
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> inviteUserToTeam([FromForm] int teamAdminId, [FromForm] int userToAddId)
        {
            var team = await _context.Teams.SingleOrDefaultAsync(x => x.AdminId == teamAdminId);
            var teamId = team.Id;
            var userTeamn = new UsersTeam()
            {
                TeamId = team.Id,
                UserId = userToAddId,
                IsPending = true,
            };
            if(userTeamn == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                await _context.UsersTeams.AddAsync(userTeamn);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<List<UsersTeam>> invitationsToTeam([FromForm] int userId)
        {
            List<UsersTeam> usersTeams = await _context.UsersTeams.Where(x => x.UserId == userId).Where(x => x.IsPending == true).ToListAsync();
            return usersTeams;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> acceptTeamInvitation([FromForm] int teamId, [FromForm] int userId)
        {
            var usersTeam = await _context.UsersTeams.Where(x => x.TeamId == teamId).Where(x => x.UserId == userId).Where(x => x.IsPending == true).SingleOrDefaultAsync();
            usersTeam.IsPending = false;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeclineTeamInvitation([FromForm] int teamId, [FromForm] int userId)
        {
            UsersTeam usersTeam = await _context.UsersTeams.Where(x => x.TeamId == teamId).Where(x => x.UserId == userId).SingleOrDefaultAsync();
            _context.UsersTeams.Remove(usersTeam);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> followTeam([FromForm] int teamId, [FromForm] int userId)
        {
            var follow = new TeamsFollower()
            {
                TeamId = teamId,
                UserId = userId,
            };
            if(follow == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                await _context.TeamsFollowers.AddAsync(follow);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
        }






    }
}
