using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorldWideGaming.Converters;

namespace WorldWideGaming.ViewModels
{
    public class ConnectionVm
    {
        [JsonConverter(typeof(IntToStringConverter))]
        public int User1 { get; set; }

        [JsonConverter(typeof(IntToStringConverter))]
        public int User2 { get; set; }
    }
}
