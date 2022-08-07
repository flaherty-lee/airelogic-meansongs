using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MeanSongs.Exceptions;
using MeanSongs.Models.Lyricsovh;
using MeanSongs.Models.MusicBrainz;
using MeanSongs.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MeanSongs.Services
{
    public class MusicbrainzService : ImusicbrainzService
    {
        private readonly HttpClient _httpClient;
        private const string format = "json";

        public MusicbrainzService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetConnectionString("MusicBrainzUri"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "aire-logic-meanlogic");
        }

        public async Task<Guid?> getArtistId(string artistName, int limit = 1)
        {
            var response = await _httpClient.GetAsync($"artist/?query={artistName}&fmt={format}&limit={limit}");

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var responseOkString = await response.Content.ReadAsStringAsync();
                    var responseOkObject = JsonSerializer.Deserialize<ArtistSearch>(responseOkString);
                    return responseOkObject?.artists.FirstOrDefault(a => a.name.Equals(artistName, StringComparison.InvariantCultureIgnoreCase))?.id;
                default:
                    throw new ApiServiceException($"Failed to find artist {artistName}", response);
            }
        }

        public async Task<List<string>?> getTitles(Guid artistGuid)
        {
            List<string> songs = new();
            int count = 0;
            int workCount = 0;

            //Get all titles, 100 songs at a time (maximum allowed by api).
            do
            {
                var response = await _httpClient.GetAsync($"work/?artist={artistGuid}&limit=100&fmt=json&offset={count}");
                var responseObject = JsonSerializer.Deserialize<Works>(await response.Content.ReadAsStringAsync());
                if(workCount == 0) { workCount = responseObject?.workCount ?? 0; }
                count += 100;

                var currentSongs = responseObject?.works
                    .Where(w => w.workType != null && w.workType.Equals("Song", StringComparison.InvariantCultureIgnoreCase))
                    .Select(w => w.title).ToList();

                if(currentSongs != null)
                {
                    songs.AddRange(currentSongs);
                }
            }
            while (count < workCount);
            return songs;
        }
    }
}
