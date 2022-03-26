using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PlayLister.Services.Models.Youtube;

namespace PlayLister.Services.Models
{
    public class YoutubeSnippet
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnails")]
        public YoutubeThumbnail Thumbnails { get; set; }
    }
}
