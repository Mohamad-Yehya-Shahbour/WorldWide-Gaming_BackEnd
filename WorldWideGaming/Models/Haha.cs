using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorldWideGaming.Models
{
    [Table("haha")]
    public partial class Haha
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("haha")]
        public int? Haha1 { get; set; }
    }
}
