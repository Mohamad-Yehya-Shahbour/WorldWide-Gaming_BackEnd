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
    [Table("posts")]
    public partial class Post
    {
        public Post()
        {
            PostsComments = new HashSet<PostsComment>();
            PostsLikes = new HashSet<PostsLike>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("description")]
        [StringLength(200)]
        public string Description { get; set; }
        [Column("clipUrl")]
        public string ClipUrl { get; set; }

        [NotMapped]
        public IFormFile formFile { get; set; }

        [Column("userId")]
        public int UserId { get; set; }
        [Column("numberOfLikes")]
        public int NumberOfLikes { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Posts")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(PostsComment.Post))]
        public virtual ICollection<PostsComment> PostsComments { get; set; }
        [InverseProperty(nameof(PostsLike.Post))]
        [JsonIgnore]
        public virtual ICollection<PostsLike> PostsLikes { get; set; }
    }
}
