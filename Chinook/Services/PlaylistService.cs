using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ChinookContext _dbContext;
        public List<Playlist> Playlists { get; private set; }
        public PlaylistService(IDbContextFactory<ChinookContext> dbFactory)
        {
            _dbContext = dbFactory.CreateDbContext();
            Playlists = _dbContext.Playlists.Include(_ => _.Tracks).ToList();  
        }

        public async Task<PlaylistTrack> AddPlaylist(PlaylistTrack playlistTrack)
        {
            var addedPlaylist = _dbContext.PlaylistTracks.Add(playlistTrack);
            await _dbContext.SaveChangesAsync();

            Playlists = _dbContext.Playlists.Include(_ => _.Tracks).ToList();

            return addedPlaylist.Entity;
        }
    }
}
