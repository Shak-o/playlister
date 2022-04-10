using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayLister.Services.Models.ServiceModels
{
    public class PlaylistServiceModel
    {
        public string Id { get; set; }
        public List<ItemServiceModel> Items { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalResults { get; set; }
        public string NextPageToken { get; set; }
        public string PreviousPageToken { get; set; }
    }
}
