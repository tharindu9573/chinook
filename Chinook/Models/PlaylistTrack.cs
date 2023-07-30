namespace Chinook.Models
{
    public class PlaylistTrack
    {
        public long PlaylistTrackId { get; set; }
        public long PlaylistId { get; set; }
        public long TrackId { get; set; }
        public Playlist Playlist { get; set; }
        public Track Track { get; set; }

    }
}
