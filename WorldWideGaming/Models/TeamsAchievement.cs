using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("teams_achievements")]
    public partial class TeamsAchievement
    {
        public TeamsAchievement()
        {
            Teams = new HashSet<Team>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("teamId")]
        public int TeamId { get; set; }
        [Column("body")]
        public string Body { get; set; }

        [InverseProperty(nameof(Team.Achievements))]
        public virtual ICollection<Team> Teams { get; set; }
    }
}
