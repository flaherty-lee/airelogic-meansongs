namespace MeanSongs.Services.Interfaces
{
    public interface ImusicbrainzService
    {
        Task<Guid?> getArtistId(string artistName, int limit = 1);

        Task<List<string>?> getTitles(Guid artistGuid);
    }
}