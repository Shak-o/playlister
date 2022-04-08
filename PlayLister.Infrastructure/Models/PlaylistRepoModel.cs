using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayLister.Infrastructure.Models
{
    public class PlaylistRepoModel
    {
        public string Id { get; set; }
        public List<ItemRepoModel> Items { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalResults { get; set; }
    }
}
