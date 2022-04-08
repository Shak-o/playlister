using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using PlayLister.Infrastructure.Models;
using PlayLister.Infrastructure.Repositories.Interfaces;
using PlayLister.Services.Helpers;
using PlayLister.Services.Interfaces;
using PlayLister.Services.Models;

namespace PlayLister.Services.Implementation
{
    public class PlaylistConverter : IPlaylistConverter
    {
        private readonly IAppDataRepository _repository;
        private readonly IPlaylistRepository _playlistRepo;
        private readonly IMapper _mapper;

        public PlaylistConverter(IAppDataRepository repository, IMapper mapper, IPlaylistRepository playlistRepo)
        {
            _repository = repository;
            _mapper = mapper;
            _playlistRepo = playlistRepo;
        }

        public async Task<PlaylistData> GetPlaylistItems(string playlistId)
        {
            var key = _repository.GetKey();

            HttpClient client = new HttpClient();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            parameters.Add("key", key.Code);
            parameters.Add("part", "snippet");
            parameters.Add("maxResults", "1000");
            parameters.Add("playlistId", playlistId);

            var uri = UriHelper.CreateUri("https://youtube.googleapis.com/youtube/v3/playlistItems", parameters);

            var response = await client.GetAsync(uri);

            var data = await JsonSerializer.DeserializeAsync<PlaylistData>(response.Content.ReadAsStream());

            data.PlaylistId = playlistId;

            await SavePlaylist(data);

            data.Items = data.Items.Take(new Range(0, 15)).ToList();

            return data;
                
        }

        public async Task<PlaylistData> GetPlaylistItems(Guid id,int page)
        {
            return null;
        }

        public async Task SavePlaylist(PlaylistData data)
        {
            var map = _mapper.Map<PlaylistData, PlaylistRepoModel>(data);
            await _playlistRepo.AddPlaylist(map);
        }

        private async Task<PlaylistData> CheckInSpotify(string accessToken, PlaylistData data)
        {
            foreach (var item in data.Items)
            {
                var validName = StringHelper.GetValidName(item.Snippet.Title);
            }

            return data;
        }

        private async Task<YoutubeData> GetYoutubePlaylist(string channelId)
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
