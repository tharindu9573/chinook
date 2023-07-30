using Chinook.Models;

namespace Chinook.Services
{
    public interface IPlaylistService
    {
        List<Playlist> Playlists { get; }
        Task<PlaylistTrack> AddPlaylist(PlaylistTrack playlistTrack);
    }
}
