using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayLister.Infrastructure.Context;
using PlayLister.Infrastructure.Models;
using PlayLister.Infrastructure.Repositories.Interfaces;

namespace PlayLister.Infrastructure.Repositories.Implementation
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly PlayListerContext _context;

        public PlaylistRepository(PlayListerContext context)
        {
            _context = context;
        }

        public async Task<PlaylistRepoModel> GetPlaylistItemsAsync(string id, int page)
        {
            var data = await _context.Playlists.FirstOrDefaultAsync(x => x.Id == id);
            var maxPage = data.Items.Count / 15;

            if (page < maxPage)
                data.Items = data.Items.Take(new Range(15 * page, 15 * (page + 1))).ToList();
            else
                data.Items = null;

            return data;
        }

        public async Task AddPlaylist(PlaylistRepoModel playlist)
        {
            try
            {
                var check = _context.Playlists.FirstOrDefault(x => x.Id == playlist.Id);
                if (check == null)
                {
                    _context.Playlists.Add(playlist);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
