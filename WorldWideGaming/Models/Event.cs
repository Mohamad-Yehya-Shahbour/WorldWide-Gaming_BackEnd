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
    [Table("events")]
    public partial class Event
    {
        public Event()
        {
            EventsJoiners = new HashSet<EventsJoiner>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("title")]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        [Column("description")]
        public string Description { get; set; }
        [Column("long", TypeName = "decimal(9, 7)")]
        public decimal Long { get; set; }
        [Column("lat", TypeName = "decimal(9, 7)")]
        public decimal Lat { get; set; }
        [Required]
        [Column("imageUrl")]
        public string ImageUrl { get; set; }

        [NotMapped]
        public IFormFile formFile {get; set;}


        [Column("joinersNum")]
        public int JoinersNum { get; set; }
        [Column("userId")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Events")]
        
        public virtual User User { get; set; }
        [InverseProperty(nameof(EventsJoiner.Event))]
        public virtual ICollection<EventsJoiner> EventsJoiners { get; set; }
    }
}
