using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MeanSongs.Models.MusicBrainz
{
    public class Works
    {
        [JsonPropertyName("works")]
        public List<Work> works { get; set; } = new();

        [JsonPropertyName("work-count")]
        public int workCount { get; set; }
    }
}
