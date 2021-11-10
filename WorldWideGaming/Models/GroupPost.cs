using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("group_posts")]
    public partial class GroupPost
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("body")]
        public string Body { get; set; }
        [Column("userId")]
        public int? UserId { get; set; }
        [Column("groupId")]
        public int? GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("GroupPosts")]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("GroupPosts")]
        public virtual User User { get; set; }
    }
}
