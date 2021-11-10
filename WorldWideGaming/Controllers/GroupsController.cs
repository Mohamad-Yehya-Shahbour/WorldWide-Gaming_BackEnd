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
using WorldWideGaming.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldWideGaming.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public GroupsController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Group>> getGroups([FromQuery] int id)
        {
            List<UsersGroup> usersGroups = await _context.UsersGroups.Where(x => x.UserId == id).ToListAsync();

            List<int> list = new List<int>();

            foreach (var usersGroup in usersGroups)
            {
                list.Add(usersGroup.GroupId);
            }

            List<Group> groups = await _context.Groups.Where(p => !list.Contains(p.Id)).Where(x => x.UserId != id).ToListAsync();

            return groups;
        } //Done

        [HttpGet]
        [Authorize]
        public async Task<List<Group>> getUserGroups([FromQuery] int userId)
        {
            List<UsersGroup> userInGroups = await _context.UsersGroups.Where(x => x.UserId == userId).ToListAsync();

            List<int> list = new List<int>();

            foreach (var userInGroup in userInGroups)
            {
                //var temp = userInGroup.GroupId;
                list.Add(userInGroup.GroupId);
            }
             
            List<Group> userGroups = await _context.Groups.Where(p => list.Contains(p.Id)).ToListAsync();

            return userGroups;
        } //Done

        [HttpGet]
        [Authorize]
        public async Task<List<Group>> getUserGroupsById([FromQuery] int userId)
        {
            return await _context.Groups.Where(x => x.UserId == userId).ToListAsync();
        }

        [HttpGet]
        [Authorize]
        public async Task<List<GroupPost>> getGroupPosts([FromQuery] int id)
        {
            List<GroupPost> groupPosts = await _context.GroupPosts.Where(x => x.GroupId == id).Include(x => x.User).ToListAsync();
            return groupPosts;
        } //Done

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> joinGroup([FromBody] UsersGroup usersGroup)
        {
            var check = await _context.UsersGroups.Where(x => x.UserId == usersGroup.UserId).Where(x => x.GroupId == usersGroup.GroupId).SingleOrDefaultAsync();
            if (check != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                var userGroup = new UsersGroup()
                {
                    UserId = usersGroup.UserId,
                    GroupId = usersGroup.GroupId
                };
                if (userGroup == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    await _context.UsersGroups.AddAsync(userGroup);
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK);
                }
            }

        } //Done

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> addPostOnGroup([FromBody] GroupPost groupPost)
        {
            var postToAdd = new GroupPost()
            {
                Body = groupPost.Body,
                UserId = groupPost.UserId,
                GroupId = groupPost.GroupId,
            };
            if (string.IsNullOrEmpty(groupPost.Body))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                await _context.GroupPosts.AddAsync(postToAdd);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
        } //Done

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> createGroup([FromForm] CreateGroupVm createGroupVm)
        {
            string uniqueId = Guid.NewGuid().ToString();
            string destination = Path.Combine("wwwroot", "pictures", uniqueId + ".jpg");
            using (FileStream fs = new FileStream(destination, FileMode.Create))
            {
                createGroupVm.formFile.CopyTo(fs);
            }
            string dbUrl = destination.Replace("wwwroot", "").Replace("\\", "/");

            var groupToAdd = new Group()
            {
                UserId = createGroupVm.UserId,
                Name = createGroupVm.Name,
                Description = createGroupVm.Description,
                Game = dbUrl,
                

            };
            if (createGroupVm != null)
            {
                await _context.Groups.AddAsync(groupToAdd);
                await _context.SaveChangesAsync();

                //var userInGroup = new UsersGroup()
                //{
                //    UserId = createGroupVm.UserId,
                //    GroupId = groupToAdd.Id,
                //};
                //await _context.UsersGroups.AddAsync(userInGroup);
                //await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }



        } //Done




    }
}
