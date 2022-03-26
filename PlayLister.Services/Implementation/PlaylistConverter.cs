using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PlayLister.Infrastructure.Repositories.Interfaces;
using PlayLister.Services.Helpers;
using PlayLister.Services.Interfaces;
using PlayLister.Services.Models;

namespace PlayLister.Services.Implementation
{
    public class PlaylistConverter : IPlaylistConverter
    {
        private readonly IAppDataRepository _repository;

        public PlaylistConverter(IAppDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<YoutubeData> GetYoutubePlaylists(string channelId)
        {
            var key = _repository.GetKey();

            HttpClient client = new HttpClient();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            parameters.Add("key",key.Code);
            parameters.Add("part","snippet");
            parameters.Add("channelId",channelId);

            var uri = UriHelper.CreateUri("https://youtube.googleapis.com/youtube/v3/playlists", parameters);

            var response = await client.GetAsync(uri);

            var data = await JsonSerializer.DeserializeAsync<YoutubeData>(response.Content.ReadAsStream());

            return data;
        }

        public Task<List<YoutubeItem>> GetPlaylistItems(string playlistId)
        {
            throw new NotImplementedException();
        }

        public Task MakeSpotifyPlaylist(string playlistId)
        {
            throw new NotImplementedException();
        }
    }
}
