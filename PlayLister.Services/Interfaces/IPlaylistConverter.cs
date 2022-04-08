using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayLister.Services.Models;

namespace PlayLister.Services.Interfaces
{
    public interface IPlaylistConverter
    {
        Task<PlaylistData> GetPlaylistItems(string platListId);
        Task<PlaylistData> GetPlaylistItems(Guid id, int page);
        Task SavePlaylist(PlaylistData data);
        Task MakeSpotifyPlaylist(string playlistId);
    }
}
