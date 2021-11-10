using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorldWideGaming.Converters;

namespace WorldWideGaming.ViewModels
{
    public class userIdVm
    {
        [JsonConverter(typeof(IntToStringConverter))]
        public int Id { get; set; }
    }
}
