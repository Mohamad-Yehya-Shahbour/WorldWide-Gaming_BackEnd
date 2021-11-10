using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("teams_followers")]
    public partial class TeamsFollower
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userId")]
        public int UserId { get; set; }
        [Column("teamId")]
        public int TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        [InverseProperty("TeamsFollowers")]
        public virtual Team Team { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("TeamsFollowers")]
        public virtual User User { get; set; }
    }
}
