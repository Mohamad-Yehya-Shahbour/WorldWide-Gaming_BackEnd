using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("users_teams")]
    public partial class UsersTeam
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userId")]
        public int UserId { get; set; }
        [Column("teamId")]
        public int TeamId { get; set; }
        [Column("isPending")]
        public bool? IsPending { get; set; }

        [ForeignKey(nameof(TeamId))]
        [InverseProperty("UsersTeams")]
        public virtual Team Team { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UsersTeams")]
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
