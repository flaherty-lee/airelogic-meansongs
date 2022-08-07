using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MeanSongs.Models.MusicBrainz
{
    public class ArtistSearch
    {
        [JsonPropertyName("artists")]
        public List<Artist> artists { get; set; } = new List<Artist>();
    }
}
