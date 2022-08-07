using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeanSongs.Services.Interfaces
{
    public interface ILyricsovh
    {
        Task<string?> GetLyrics(string artistName, string title, int retries = 3);
    }
}
