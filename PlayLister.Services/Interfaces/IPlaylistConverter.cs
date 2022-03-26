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
        Task<PlaylistData> GetPlaylistItems(string channelId, string playlistName);
        Task MakeSpotifyPlaylist(string playlistId);
    }
}
