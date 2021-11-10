using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("posts_likes")]
    public partial class PostsLike
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userId")]
        public int UserId { get; set; }
        [Column("postId")]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty("PostsLikes")]
        public virtual Post Post { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("PostsLikes")]
        public virtual User User { get; set; }
    }
}
