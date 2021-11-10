using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("notifications")]
    public partial class Notification
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user1Id")]
        public int? User1Id { get; set; }
        [Column("user2Id")]
        public int? User2Id { get; set; }

        [ForeignKey(nameof(User1Id))]
        [InverseProperty(nameof(User.NotificationUser1s))]
        public virtual User User1 { get; set; }
        [ForeignKey(nameof(User2Id))]
        [InverseProperty(nameof(User.NotificationUser2s))]
        public virtual User User2 { get; set; }
    }
}
