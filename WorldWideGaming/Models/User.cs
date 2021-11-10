using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("users")]
    public partial class User
    {
        public User()
        {
            Events = new HashSet<Event>();
            EventsJoiners = new HashSet<EventsJoiner>();
            FriendUserOnes = new HashSet<Friend>();
            FriendUserTwos = new HashSet<Friend>();
            GroupPosts = new HashSet<GroupPost>();
            Groups = new HashSet<Group>();
            NotificationUser1s = new HashSet<Notification>();
            NotificationUser2s = new HashSet<Notification>();
            Posts = new HashSet<Post>();
            PostsComments = new HashSet<PostsComment>();
            PostsLikes = new HashSet<PostsLike>();
            Teams = new HashSet<Team>();
            TeamsFollowers = new HashSet<TeamsFollower>();
            UsersGroups = new HashSet<UsersGroup>();
            UsersTeams = new HashSet<UsersTeam>();
        }

        [Key]
        public int Id { get; set; }
        [Column("userName")]
        [StringLength(35)]
        public string UserName { get; set; }
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }
        [Column("password")]
        [StringLength(400)]
        public string Password { get; set; }
        [Column("country")]
        [StringLength(50)]
        public string Country { get; set; }
        [Column("region")]
        [StringLength(50)]
        public string Region { get; set; }
        [Column("dob", TypeName = "date")]
        public DateTime? Dob { get; set; }
        [Column("rating")]
        public int? Rating { get; set; }
        [Column("gender")]
        [StringLength(10)]
        public string Gender { get; set; }
        [Column("imageUrl")]
        public string ImageUrl { get; set; }

        [NotMapped]
        public IFormFile formFile { get; set; }


        [Column("isDeleted")]
        public bool? IsDeleted { get; set; }
        [Column("isActivate")]
        public bool? IsActivate { get; set; }
        [Column("isGamingCenter")]
        public bool? IsGamingCenter { get; set; }
        [Column("isTeamAdmin")]
        public bool? IsTeamAdmin { get; set; }
        [Column("isInTeam")]
        public bool? IsInTeam { get; set; }

        [InverseProperty(nameof(Event.User))]
        [JsonIgnore]
        public virtual ICollection<Event> Events { get; set; }
        [InverseProperty(nameof(EventsJoiner.User))]
        [JsonIgnore]
        public virtual ICollection<EventsJoiner> EventsJoiners { get; set; }
        [InverseProperty(nameof(Friend.UserOne))]
        [JsonIgnore]
        public virtual ICollection<Friend> FriendUserOnes { get; set; }
        [InverseProperty(nameof(Friend.UserTwo))]
        [JsonIgnore]
        public virtual ICollection<Friend> FriendUserTwos { get; set; }
        [InverseProperty(nameof(GroupPost.User))]
        [JsonIgnore]
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
        [InverseProperty(nameof(Group.User))]
        [JsonIgnore]
        public virtual ICollection<Group> Groups { get; set; }
        [InverseProperty(nameof(Notification.User1))]
        [JsonIgnore]
        public virtual ICollection<Notification> NotificationUser1s { get; set; }
        [InverseProperty(nameof(Notification.User2))]
        [JsonIgnore]
        public virtual ICollection<Notification> NotificationUser2s { get; set; }
        [InverseProperty(nameof(Post.User))]
        [JsonIgnore]
        public virtual ICollection<Post> Posts { get; set; }
        [InverseProperty(nameof(PostsComment.UserWhoComments))]
        [JsonIgnore]
        public virtual ICollection<PostsComment> PostsComments { get; set; }
        [InverseProperty(nameof(PostsLike.User))]
        [JsonIgnore]
        public virtual ICollection<PostsLike> PostsLikes { get; set; }
        [InverseProperty(nameof(Team.Admin))]
        [JsonIgnore]
        public virtual ICollection<Team> Teams { get; set; }
        [InverseProperty(nameof(TeamsFollower.User))]
        [JsonIgnore]
        public virtual ICollection<TeamsFollower> TeamsFollowers { get; set; }
        [InverseProperty(nameof(UsersGroup.User))]
        [JsonIgnore]
        public virtual ICollection<UsersGroup> UsersGroups { get; set; }
        [InverseProperty(nameof(UsersTeam.User))]
        [JsonIgnore]
        public virtual ICollection<UsersTeam> UsersTeams { get; set; }
    }
}
