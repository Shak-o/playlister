using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayLister.Infrastructure.Models
{
    public class ItemRepoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public PlaylistRepoModel Playlist { get; set; }
        public string PlaylistId { get; set; }
    }
}
