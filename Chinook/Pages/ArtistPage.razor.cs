using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Services;
using Chinook.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Playlist = Chinook.Models.Playlist;
using PlaylistTrack = Chinook.ClientModels.PlaylistTrack;

namespace Chinook.Pages
{
    public partial class ArtistPage
    {
        [Parameter]
        public long ArtistId { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationState { get; set; }

        [Inject]
        IDbContextFactory<ChinookContext> DbFactory { get; set; }

        [Inject]
        IPlaylistService PlaylistService { get; set; } 

        private Modal PlaylistDialog { get; set; }

        private Artist Artist;
        private List<ClientModels.PlaylistTrack> Tracks;
        private ChinookContext DbContext;
        private ClientModels.PlaylistTrack SelectedTrack;
        private string InfoMessage;
        private string CurrentUserId;
        private string? NewPlayList;
        private long SelectedPlaylist;
        private List<Playlist> PlayLists;
        protected override async Task OnInitializedAsync()
        {
            await InvokeAsync(StateHasChanged);
            CurrentUserId = await GetUserId();
            DbContext = await DbFactory.CreateDbContextAsync();
            Artist = DbContext.Artists.SingleOrDefault(a => a.ArtistId == ArtistId);
            Tracks = DbContext.Tracks.Where(a => a.Album.ArtistId == ArtistId).Include(a => a.Album).Select(t => new PlaylistTrack() { AlbumTitle = (t.Album == null ? "-" : t.Album.Title), TrackId = t.TrackId, TrackName = t.Name, IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == CurrentUserId && up.Playlist.Name == "Favorites")).Any() }).ToList();
            PlayLists = PlaylistService.Playlists;
        }

        private async Task<string> GetUserId()
        {
            var user = (await authenticationState).User;
            var userId = user.FindFirst(u => u.Type.Contains(ClaimTypes.NameIdentifier))?.Value;
            return userId;
        }

        private void FavoriteTrack(long trackId)
        {
            var track = Tracks.FirstOrDefault(t => t.TrackId == trackId);
            InfoMessage = $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} added to playlist Favorites.";
        }

        private void UnfavoriteTrack(long trackId)
        {
            var track = Tracks.FirstOrDefault(t => t.TrackId == trackId);
            InfoMessage = $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} removed from playlist Favorites.";
        }

        private void OpenPlaylistDialog(long trackId)
        {
            CloseInfoMessage();
            SelectedTrack = Tracks.FirstOrDefault(t => t.TrackId == trackId);
            PlaylistDialog.Open();
        }

        private void AddTrackToPlaylist()
        {
            CloseInfoMessage();
            long playlistId;
            var track = DbContext.Tracks.FirstOrDefault(_ => _.TrackId.Equals(SelectedTrack.TrackId))!;
            if (NewPlayList is not default(string) && !PlayLists.Any(_ => _.Name!.Equals(NewPlayList)))
            {
                playlistId = PlayLists.Max(_ => _.PlaylistId) + 1;
                DbContext.Playlists.Add(new()
                {
                    PlaylistId = playlistId,
                    Name = NewPlayList
                });

                DbContext.SaveChanges();
            }
            else
                playlistId = SelectedPlaylist;

            PlaylistService.AddPlaylist(new Models.PlaylistTrack { 
                PlaylistId = playlistId,
                TrackId = track.TrackId
            });

            InfoMessage = $"Track {Artist.Name} - {SelectedTrack.AlbumTitle} - {SelectedTrack.TrackName} added to playlist {{playlist name}}.";
            PlaylistDialog.Close();
        }

        private void CloseInfoMessage()
        {
            InfoMessage = "";
        }
    }
}