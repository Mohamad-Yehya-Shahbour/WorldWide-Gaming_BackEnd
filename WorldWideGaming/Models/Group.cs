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
    [Table("groups")]
    public partial class Group
    {
        public Group()
        {
            GroupPosts = new HashSet<GroupPost>();
            UsersGroups = new HashSet<UsersGroup>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(200)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(400)]
        public string Description { get; set; }
        [Column("game")]
        [StringLength(200)]
        public string Game { get; set; }
        

        [Column("userId")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Groups")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(GroupPost.Group))]
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
        [InverseProperty(nameof(UsersGroup.Group))]
        [JsonIgnore]
        public virtual ICollection<UsersGroup> UsersGroups { get; set; }
    }
}
