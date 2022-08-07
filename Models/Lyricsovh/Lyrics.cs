using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MeanSongs.Models.Lyricsovh
{
    public class LyricResponse
    {
        [JsonPropertyName("lyrics")]
        public string lyrics { get; set; }
    }
}
