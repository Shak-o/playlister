using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlayLister.Services.Models
{
    public class PlaylistData
    {
        [JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonPropertyName("previousPageToken")]
        public string PreviousPageToken { get; set; }

        [JsonPropertyName("items")]
        public List<YoutubeItem> Items { get; set; }

        public string PlaylistId { get; set; }
    }
}
