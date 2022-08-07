using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MeanSongs.Models.MusicBrainz
{
    public class Artist
    {
        [JsonPropertyName("id")]
        public Guid id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; } = string.Empty;
    }
}
