using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using PlayLister.Infrastructure.Models;
using PlayLister.Infrastructure.Repositories.Interfaces;
using PlayLister.Services.Helpers;
using PlayLister.Services.Infrastructure;
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
                var data = await GetPlaylistDataPerPage(playlistId, 0);
                return data;
            }
        }

        public async Task<PlaylistServiceModel> GetPlaylistDataPerPage(string id,int page)
        {
            var data = await _playlistRepo.GetPlaylistItemsAsync(id, page);
            var map = _mapper.Map<PlaylistRepoModel, PlaylistServiceModel>(data);
            return map;
        }

        private async Task<List<String>> CheckInSpotify(string accessToken, PlaylistServiceModel data)
        {
            try
            {
                var toReturn = new List<string>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("type", "track");

                foreach (var item in data.Items)
                {
                    var validName = StringHelper.GetValidName(item.Title);

                    if (parameters.ContainsKey("q"))
                        parameters.Remove("q");

                    parameters.Add("q", validName);

                    var uri = UriHelper.CreateUri("https://api.spotify.com/v1/search", parameters);

                    var response = await client.GetAsync(uri);

                    var convert = await JsonSerializer.DeserializeAsync<SearchResult>(response.Content.ReadAsStream());

                    foreach (var music in convert!.Result.MusicList)
                    {
                        string firstPart = string.Empty;
                        string secondPart = string.Empty;

                        if (validName.Contains('-'))
                        {
                            int index = validName.IndexOf('-');
                            firstPart = validName.Substring(index + 1, validName.Length - index - 1).ToLower().Trim();
                            secondPart = validName.Substring(0, index).ToLower().Trim();
                        }

                        string lowerName = music.Name.ToLower();

                        if (lowerName.Like($"%{validName}%") || lowerName.Contains(firstPart) ||
                            lowerName.Contains(secondPart))
                        {
                            toReturn.Add(music.Uri);
                            break;
                        }
                    }
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in CheckinSpotify: " + ex.Message);
            }
           
        }

        private async Task<YoutubeData> GetYoutubePlaylist(string channelId)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error in GetYoutubePlaylist: " + ex.Message);
            }
            
        }

        public async Task MakeSpotifyPlaylist(string playlistId, string accessToken)
        {
            try
            {
                var data = await _playlistRepo.GetFullPlaylist(playlistId);
                var foundMusic = await CheckInSpotify(accessToken,
                    _mapper.Map<PlaylistRepoModel, PlaylistServiceModel>(data));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync("https://api.spotify.com/v1/me");
                var userData = await JsonSerializer.DeserializeAsync<User>(response.Content.ReadAsStream());

                var toPost = new {name = Guid.NewGuid().ToString()};
                if (userData != null)
                {
                    var createResponse = await client.PostAsync($"https://api.spotify.com/v1/users/{userData.Id}/playlists", new StringContent(JsonSerializer.Serialize(toPost)));
                    var playlist = await JsonSerializer.DeserializeAsync<PlaylistResponse>(createResponse.Content.ReadAsStream());
                    if (foundMusic.Count > 100)
                    {
                        for (int i = 0; i <= foundMusic.Count / 100; i++)
                        {
                            if (i * 100 < foundMusic.Count)
                            {
                                var addItemData = new { uris = foundMusic.Take(new Range(100 * i, 100 * (i + 1))) };
                                await client.PostAsync($"https://api.spotify.com/v1/playlists/{playlist.Id}/tracks", new StringContent(JsonSerializer.Serialize(addItemData)));
                            }
                            else
                            {
                                var addItemData = new { uris = foundMusic.Take(new Range(100 * i, foundMusic.Count)) };
                                await client.PostAsync($"https://api.spotify.com/v1/playlists/{playlist.Id}/tracks", new StringContent(JsonSerializer.Serialize(addItemData)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened in MakeSpotifyPlaylist: " + ex.Message);
            }
        }


        public async Task RemoveItemFromPlaylist(int id, string playlistId)
        {
            await _playlistRepo.RemoveItem(id, playlistId);
        }
    }
}
