using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MeanSongs.Models.MusicBrainz
{
    public class Work
    {
        [JsonPropertyName("id")]
        public Guid id { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; } = String.Empty;

        [JsonPropertyName("type")]
        public string workType { get; set; } = String.Empty;
    }
}
