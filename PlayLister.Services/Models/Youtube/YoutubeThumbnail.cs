using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlayLister.Services.Models.Youtube
{
    public class YoutubeThumbnail
    {
        [JsonPropertyName("default")]
        public YoutubeDefault ThumbnailDefault { get; set; }
    }
}
