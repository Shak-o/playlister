using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlayLister.Services.Models
{
    public class YoutubeSnippet
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
