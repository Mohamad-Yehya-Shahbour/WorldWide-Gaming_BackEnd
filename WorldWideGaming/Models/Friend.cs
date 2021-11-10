using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("friends")]
    public partial class Friend
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userOneId")]
        public int UserOneId { get; set; }
        [Column("userTwoId")]
        public int UserTwoId { get; set; }
        [Column("isPending")]
        public int? IsPending { get; set; }

        [ForeignKey(nameof(UserOneId))]
        [InverseProperty(nameof(User.FriendUserOnes))]
        public virtual User UserOne { get; set; }
        [ForeignKey(nameof(UserTwoId))]
        [InverseProperty(nameof(User.FriendUserTwos))]
        public virtual User UserTwo { get; set; }
    }
}
