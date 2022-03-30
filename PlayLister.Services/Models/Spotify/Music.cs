using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PlayLister.Services.Models.Spotify;

namespace PlayLister.Services.Models
{
    //[JsonProperty]
    public class Music
    {
        [JsonPropertyName("artists")]
        public List<Artist> Artists { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("popularity")]
        public int Popularity { get; set; }
    }
}
