using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorldWideGaming.Converters;

namespace WorldWideGaming.ViewModels
{
    public class CreateGroupVm
    {
        [JsonConverter(typeof(IntToStringConverter))]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Game { get; set; }

        [NotMapped]
        public IFormFile formFile { get; set; }
    }
}
