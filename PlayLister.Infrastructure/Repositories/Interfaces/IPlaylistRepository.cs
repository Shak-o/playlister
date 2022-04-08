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
        Task AddPlaylist(PlaylistRepoModel playlist);
    }
}
