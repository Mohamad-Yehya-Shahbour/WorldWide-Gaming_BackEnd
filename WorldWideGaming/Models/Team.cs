using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("teams")]
    public partial class Team
    {
        public Team()
        {
            TeamsFollowers = new HashSet<TeamsFollower>();
            UsersTeams = new HashSet<UsersTeam>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(200)]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("achievementsId")]
        public int AchievementsId { get; set; }
        [Column("adminId")]
        public int AdminId { get; set; }

        [ForeignKey(nameof(AchievementsId))]
        [InverseProperty(nameof(TeamsAchievement.Teams))]
        public virtual TeamsAchievement Achievements { get; set; }
        [ForeignKey(nameof(AdminId))]
        [InverseProperty(nameof(User.Teams))]
        public virtual User Admin { get; set; }
        [InverseProperty(nameof(TeamsFollower.Team))]
        public virtual ICollection<TeamsFollower> TeamsFollowers { get; set; }
        [InverseProperty(nameof(UsersTeam.Team))]
        public virtual ICollection<UsersTeam> UsersTeams { get; set; }
    }
}
