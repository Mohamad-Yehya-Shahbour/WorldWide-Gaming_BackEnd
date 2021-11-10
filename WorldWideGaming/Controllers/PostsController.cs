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
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public PostsController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        [Authorize]
        public async Task<List<Post>> friendsPosts([FromQuery] int id)
        {
            
            List<Friend> friend1 = await _context.Friends.Where(x => x.UserOneId == id).Where(x => x.IsPending == 0).ToListAsync();
            List<Friend> friend2 = await _context.Friends.Where(x => x.UserTwoId == id).Where(x => x.IsPending == 0).ToListAsync();
            List<Friend> friends = new List<Friend>();
            if (friend1.Count() == 0)
            {
                friends = friend2;
            }else if(friend2.Count() == 0)
            {
                friends = friend1;
            }
            else
            {
                friends = friend1.Union(friend2).ToList();
            }
            if(friends.Count() == 0)
            {
                List<Post> userPosts = await _context.Posts.Where(p => p.UserId == id).Include(x => x.User).Include(x => x.PostsComments).ToListAsync();
                return userPosts;
            }

            List<int> list = new List<int>();

            //var temp = 0;
            //foreach (var friend in friends)
            //{
            //    temp = friend.UserOneId;
            //    if (temp != id)
            //    {
            //        temp = friend.UserTwoId;
            //    }
            //    list.Add(temp);
            //}
            foreach (var friend in friends)
            {
                list.Add(friend.UserOneId);
                list.Add(friend.UserTwoId);
            }
            list = list.Distinct().ToList();

            List<Post> posts = _context.Posts.Where(p => list.Contains(p.UserId)).Include(x => x.User).Include(x => x.PostsComments).ToList();
            return posts;
        } //Done

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> addFriends ([FromBody] ConnectionVm connectionVm )
        {
            var addFriend = new Friend()
            {
                UserOneId = connectionVm.User1,
                UserTwoId = connectionVm.User2,
                IsPending = 1,
            };
            if(addFriend == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                await _context.Friends.AddAsync(addFriend);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> acceptFriend([FromBody] ConnectionVm connectionVm)
        {
            var accept = await _context.Friends.Where(x => x.UserOneId == connectionVm.User2).Where(x => x.UserTwoId == connectionVm.User1).SingleOrDefaultAsync();
            if (accept == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                accept.IsPending = 0;
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> declineFriend([FromBody] ConnectionVm connectionVm)
        {
            var decline = await _context.Friends.Where(x => x.UserOneId == connectionVm.User2).Where(x => x.UserTwoId == connectionVm.User1).SingleOrDefaultAsync();
            if (decline == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                _context.Friends.Remove(decline);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Friend>> getFriendRequests([FromQuery] int userId)
        {
            List<Friend> friends = await _context.Friends.Where(x => x.UserTwoId == userId).Where(x => x.IsPending == 1).Include(x=>x.UserOne).ToListAsync();
            return friends;
        }


        [HttpGet]
        [Authorize]
        public async Task<List<Post>> userPosts([FromQuery] int userId)
        {
            List<Post> posts = await _context.Posts.Where(p => p.UserId == userId).Include(x => x.User).ToListAsync();
            return posts;
        } //Done

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> addPost([FromForm] Post post)
        {
            string uniqueId = Guid.NewGuid().ToString();
            string destination = Path.Combine("wwwroot", "clips", uniqueId + ".mp4");
            using (FileStream fs = new FileStream(destination, FileMode.Create))
            {
                post.formFile.CopyTo(fs);
            }
            string dbUrl = destination.Replace("wwwroot", "").Replace("\\", "/");


            var postToAdd = new Post()
            {
                Description = post.Description,
                ClipUrl = dbUrl,
                UserId = post.UserId,
                NumberOfLikes = 0,
            };
            if (postToAdd == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                await _context.Posts.AddAsync(postToAdd);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created);
            }

        } //Done

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> addPostComment([FromBody] PostsComment postsComment)
        {
            var comment = new PostsComment()
            {
                PostId = postsComment.PostId,
                Body = postsComment.Body,
                UserWhoCommentsId = postsComment.UserWhoCommentsId,
            };

            if (string.IsNullOrEmpty(postsComment.Body))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                await _context.PostsComments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }

        } //Done

        [HttpGet]
        [Authorize]
        public async Task<List<PostsComment>> getPostComments([FromQuery] int postId)
        {
            List<PostsComment> postsComments = await _context.PostsComments.Where(x => x.PostId == postId).Include(x => x.UserWhoComments).ToListAsync();
            return postsComments;
        } //Done

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> likePost([FromBody] PostsLike postsLike)
        {
            var isexist = await _context.PostsLikes.Where(x => x.UserId == postsLike.UserId).Where(x => x.PostId == postsLike.PostId).FirstOrDefaultAsync();
            if(isexist == null)
            {
                var like = new PostsLike()
                {
                    PostId = postsLike.PostId,
                    UserId = postsLike.UserId,
                };
                if (like == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    await _context.PostsLikes.AddAsync(like);
                    await _context.SaveChangesAsync();
                    var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == postsLike.PostId);
                    if (post == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest);
                    }
                    else
                    {
                        var likes = post.NumberOfLikes;
                        post.NumberOfLikes = likes + 1;
                        await _context.SaveChangesAsync();
                        return StatusCode(StatusCodes.Status201Created);
                    }
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK);
            }
        } //Done

        [HttpGet]
        [Authorize]
        public async Task<List<Post>> getTopPosts()
        {
            List<Post> posts = await _context.Posts.OrderByDescending(x => x.NumberOfLikes).Include(x => x.User).ToListAsync();
            return posts;
        } //Done


    }
}
