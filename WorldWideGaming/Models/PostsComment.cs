using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("posts_comments")]
    public partial class PostsComment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("postId")]
        public int PostId { get; set; }
        [Column("body")]
        [StringLength(200)]
        public string Body { get; set; }
        [Column("userWhoCommentsId")]
        public int UserWhoCommentsId { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty("PostsComments")]
        [JsonIgnore]
        public virtual Post Post { get; set; }
        [ForeignKey(nameof(UserWhoCommentsId))]
        [InverseProperty(nameof(User.PostsComments))]
        public virtual User UserWhoComments { get; set; }
    }
}
