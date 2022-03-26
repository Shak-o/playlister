using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlayLister.Services.Models
{
    public class YoutubeItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("snippet")]
        public YoutubeSnippet Snippet { get; set; }
    }
}
