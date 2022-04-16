using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayLister.Infrastructure.Models;

namespace PlayLister.Infrastructure.Repositories.Interfaces
{
    public interface IPlaylistRepository
    {
        Task<PlaylistRepoModel> GetPlaylistItemsAsync(string id,int page);
        Task<PlaylistRepoModel> GetFullPlaylist(string id);
        Task RemoveItem(int id, string playlistId);
        Task AddPlaylist(PlaylistRepoModel playlist);
        Task AddOnlyItem(PlaylistRepoModel playlist);
        Task<bool> CheckIfExists(string id);
    }
}
