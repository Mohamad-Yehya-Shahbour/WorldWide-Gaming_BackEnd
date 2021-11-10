using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("events_joiners")]
    public partial class EventsJoiner
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("eventId")]
        public int EventId { get; set; }
        [Column("userId")]
        public int UserId { get; set; }

        [ForeignKey(nameof(EventId))]
        [InverseProperty("EventsJoiners")]
        [JsonIgnore]
        public virtual Event Event { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("EventsJoiners")]
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
