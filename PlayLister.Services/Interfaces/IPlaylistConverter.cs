﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayLister.Services.Models;

namespace PlayLister.Services.Interfaces
{
    public interface IPlaylistConverter
    {
        Task<YoutubeData> GetYoutubePlaylists(string channelName);
        Task<List<YoutubeItem>> GetPlaylistItems(string playlistId);
        Task MakeSpotifyPlaylist(string playlistId);
    }
}