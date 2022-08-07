using MeanSongs.Exceptions;
using MeanSongs.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeanSongs
{
    public  class MeanWorker
    {
        private readonly ImusicbrainzService _musicbrainzService;
        private readonly ILyricsovh _lyricsovhService;
        private readonly ICommandLineService _console;
        private int numberOfTitle = 0;
        private int titleCompletedCount = 0;

        public MeanWorker(ImusicbrainzService musicbrainzService, ILyricsovh lyricsovhService, ICommandLineService commandLineService)
        {
            _musicbrainzService = musicbrainzService;
            _lyricsovhService = lyricsovhService;
            _console = commandLineService;
        }

        public async Task Execute()
        {
            try
            {
                var artistName = retrieveArtistName();
                var artistId = await retrieveArtistId(artistName);
                var titles = await retrieveTitles(artistId, artistName);
                numberOfTitle = titles.Count();
                titleCompletedCount = 0;

                //Retrieve lyrics for each title and remove empty/null titles
                List<string?> lyrics = new();
                await Task.WhenAll(titles.Select(async t => lyrics.Add(await getLyrics(artistName, t))));
                lyrics = lyrics.Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

                //Calulate Mean
                _console.WriteLine($"Number of titles found for {artistName} = {lyrics.Count}.");
                double mean = lyrics.Sum(l => SplitSentenceIntoWordArray(l).Length) 
                            / lyrics.Count;
                _console.WriteLine($"Mean number of words in each song for artist {artistName} = {mean}.");
            }
            catch(Exception ex)
            {
                if(ex is ApiServiceException || ex is RetriesCountException || ex is ValidationException)
                {
                    _console.WriteLine(ex.Message);
                    return;
                }
                throw;
            }
        }

        private string[] SplitSentenceIntoWordArray(string sentence)
        {
            return sentence.Split(new char[] { ' ', '\n' });
        }

        private async Task<List<string>> retrieveTitles(Guid artistId, string artistName)
        {
            var titles = await _musicbrainzService.getTitles(artistId);
            Log.Information($"Number of titles for artist {artistName} = " + titles?.Count.ToString());
            if (titles == null || !titles.Any())
            {
                throw new ValidationException($"No songs found for artist {artistName}.");
            }
            return titles;
        }

        private async Task<Guid> retrieveArtistId(string artistName)
        {
            var artistId = await _musicbrainzService.getArtistId(artistName);
            if (artistId == null)
            {
                throw new ValidationException($"Artist {artistName} not found.");
            }
            return (Guid)artistId;
        }

        private string retrieveArtistName()
        {
            _console.Write("Please enter the name of an artist: ");
            var artistName = _console.ReadLine();
            if (string.IsNullOrWhiteSpace(artistName))
            {
                throw new ValidationException($"Artist {artistName} not found.");
            }
            return artistName;
        }

        private async Task<string?> getLyrics(string artistName, string title)
        {
            Log.Information($"Started api call for : {artistName}, {title}");
            _console.WriteLine($"Progress = " + String.Format("{0:0.00}%.", ++titleCompletedCount * 100 / (double)numberOfTitle / 2));
            var lyrics = await _lyricsovhService.GetLyrics(artistName, title);
            _console.WriteLine($"Progress = " + String.Format("{0:0.00}%.", ++titleCompletedCount * 100 / (double)numberOfTitle / 2));
            Log.Information($"Finished api call for : {artistName}, {title}");
            return lyrics;
        }
    }
}
