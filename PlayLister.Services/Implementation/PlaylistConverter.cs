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
        private string _playlistId;

        public PlaylistConverter(IAppDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<PlaylistData> GetPlaylistItems(string channelId, string playlistName)
        {
            var playlists = await GetYoutubePlaylists(channelId);

            bool found = false;

            foreach (var list in playlists.PlayLists)
            {
                if (list.Snippet.Title == playlistName)
                {
                    found = true;
                    _playlistId = list.Id;
                }
            }

            if (!found)
                throw new Exception("Playlist not found or is not public");

            var key = _repository.GetKey();

            HttpClient client = new HttpClient();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("key", key.Code);
            parameters.Add("part", "snippet");
            parameters.Add("maxResults", "15");
            parameters.Add("playlistId", _playlistId);

            var uri = UriHelper.CreateUri("https://youtube.googleapis.com/youtube/v3/playlists", parameters);

            var response = await client.GetAsync(uri);

            var data = await JsonSerializer.DeserializeAsync<PlaylistData>(response.Content.ReadAsStream());

            data.PlaylistId = _playlistId;

            return data;
        }
        private async Task<YoutubeData> GetYoutubePlaylists(string channelId)
        {
            var key = _repository.GetKey();

            HttpClient client = new HttpClient();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("key", key.Code);
            parameters.Add("part", "snippet");
            parameters.Add("channelId", channelId);

            var uri = UriHelper.CreateUri("https://youtube.googleapis.com/youtube/v3/playlists", parameters);

            var response = await client.GetAsync(uri);

            var data = await JsonSerializer.DeserializeAsync<YoutubeData>(response.Content.ReadAsStream());

            return data;
        }

        public Task MakeSpotifyPlaylist(string playlistId)
        {
            throw new NotImplementedException();
        }
    }
}
