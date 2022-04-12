using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using PlayLister.Infrastructure.Models;
using PlayLister.Infrastructure.Repositories.Interfaces;
using PlayLister.Services.Helpers;
using PlayLister.Services.Interfaces;
using PlayLister.Services.Models;
using PlayLister.Services.Models.ServiceModels;
using PlayLister.Services.Models.Spotify;

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

        public async Task<PlaylistServiceModel> GetPlaylistItems(string playlistId)
        {
            var key = _repository.GetKey();
            if (!await _playlistRepo.CheckIfExists(playlistId))
            {
                HttpClient client = new HttpClient();

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("key", key.Code);
                parameters.Add("part", "snippet");
                parameters.Add("maxResults", "50");
                parameters.Add("playlistId", playlistId);

                var uri = UriHelper.CreateUri("https://youtube.googleapis.com/youtube/v3/playlistItems", parameters);

                var response = await client.GetAsync(uri);

                var data = await JsonSerializer.DeserializeAsync<PlaylistData>(response.Content.ReadAsStream());
                PlaylistData? dataSec = new PlaylistData();
                data.PlaylistId = playlistId;
                var dataToReturn = new PlaylistServiceModel();
                if (data != null)
                {
                    var map = _mapper.Map<PlaylistData, PlaylistRepoModel>(data);
                    await _playlistRepo.AddPlaylist(map);
                    dataToReturn = _mapper.Map<PlaylistRepoModel, PlaylistServiceModel>(map);
                    data.Items = data.Items.Take(new Range(0, 15)).ToList();
                }
                else
                {
                    throw new Exception("was returned nothing");

                }

                if (data.PageInfo.TotalResults >= 50)
                {
                    var count = data.PageInfo.TotalResults / data.PageInfo.ResultsPerPage;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataSec.NextPageToken == string.Empty || dataSec.NextPageToken == null)
                        {
                            parameters.Add("pageToken", data.NextPageToken);
                        }
                        else
                        {
                            parameters.Remove("pageToken");
                            parameters.Add("pageToken", dataSec.NextPageToken);
                        }

                        uri = UriHelper.CreateUri("https://youtube.googleapis.com/youtube/v3/playlistItems",
                            parameters);
                        response = await client.GetAsync(uri);
                        dataSec = await JsonSerializer.DeserializeAsync<PlaylistData>(response.Content.ReadAsStream());
                        dataSec.PlaylistId = playlistId;
                        if (dataSec != null)
                        {
                            var map = _mapper.Map<PlaylistData, PlaylistRepoModel>(dataSec);
                            await _playlistRepo.AddOnlyItem(map);
                        }
                    }
                }

                return dataToReturn;
            }
            else
            {
                var data = await GetPlaylistItems(playlistId, 0);
                return data;
            }
        }

        public async Task<PlaylistServiceModel> GetPlaylistItems(string id,int page)
        {
            var data = await _playlistRepo.GetPlaylistItemsAsync(id, page);
            var map = _mapper.Map<PlaylistRepoModel, PlaylistServiceModel>(data);
            return map;
        }

        private async Task<PlaylistServiceModel> CheckInSpotify(string accessToken, PlaylistServiceModel data)
        {
            foreach (var item in data.Items)
            {
                var validName = StringHelper.GetValidName(item.Title);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("type", "track");
                parameters.Add("q", validName);

                var uri = UriHelper.CreateUri("https://api.spotify.com/v1/search", parameters);

                var response = await client.GetAsync(uri);

                var convert = await JsonSerializer.DeserializeAsync<SearchResult>(response.Content.ReadAsStream());
                if (convert.Result.MusicList.Any(x => x.Name.ToLower() == validName))
                {
                    //TODO:Filter logic here
                }
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

        public async Task MakeSpotifyPlaylist(string playlistId, string accessToken, PlaylistServiceModel playlistData)
        {
            await CheckInSpotify(accessToken, playlistData);
        }
    }
}
