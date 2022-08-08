using MeanSongs.Exceptions;
using MeanSongs.Models.Lyricsovh;
using MeanSongs.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net.Sockets;
using System.Text.Json;

namespace MeanSongs.Services
{
    public class LyricsovhService : ILyricsovh
    {
        private readonly HttpClient _httpClient;

        public LyricsovhService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetConnectionString("LyricsUri"));
        }

        public async Task<string?> GetLyrics(string artistName, string title, int retries = 3)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{artistName}/{title}");
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var responseOkString = await response.Content.ReadAsStringAsync();
                        var responseOkObject = JsonSerializer.Deserialize<LyricResponse>(responseOkString);
                        return responseOkObject?.lyrics;
                    case System.Net.HttpStatusCode.NotFound:
                        return null;
                    case System.Net.HttpStatusCode.BadGateway:
                        if (retries <= 0) { throw new RetriesCountException($"Failed to retrieve lyrics for title {title}."); }
                        return await GetLyrics(artistName, title, retries - 1);
                    default:
                        throw new ApiServiceException($"Failed to retrieve lyrics for {title}.", response);
                }
            }
            catch (Exception ex)
            {
                if(ex is SocketException || ex is HttpRequestException)
                {
                    Log.Warning($"GET lyrics for title {title} failed. Retries left: {retries}.");
                    Log.Warning(ex?.Message);
                    if (retries > 0) { return await GetLyrics(artistName, title, retries - 1); }
                    else { throw new RetriesCountException($"Failed to retrieve lyrics for title {title}."); }
                }

                Log.Error(ex?.Message);
                throw;
            }
        }
    }
}
