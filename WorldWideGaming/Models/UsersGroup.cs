using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("users_groups")]
    public partial class UsersGroup
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userId")]
        public int UserId { get; set; }
        [Column("groupId")]
        public int GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("UsersGroups")]
        [JsonIgnore]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UsersGroups")]
        public virtual User User { get; set; }
    }
}
