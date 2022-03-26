using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlayLister.Services.Models
{
    public class YoutubeData
    {
        [JsonPropertyName("items")]
        public List<YoutubePlaylist> PlayLists { get; set; }
    }
}
