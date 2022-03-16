﻿using PlayLister.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PlayLister.Infrastructure.Context;
using PlayLister.Infrastructure.Models;
using PlayLister.Services.Helpers;
using PlayLister.Services.Models;

namespace PlayLister.Services.Implementation
{
    public class AuthService : IAuthService
    {
        public PlayListerContext _context;

        public AuthService(PlayListerContext context)
        {
            _context = context;
        }

        public string GetUri()
        {
            var things = new Dictionary<string, string>();
            things.Add("client_id", GetClientId().ClientId);
            things.Add("response_type","code");
            things.Add("scope", "playlist-modify-public");
            things.Add("redirect_uri", "https://localhost:7150/auth/id");
            var uri = UriHelper.CreateUri("https://accounts.spotify.com/authorize", things);
            return uri;
        }

        public async Task<AuthData> RequestToken(string code)
        {
            HttpClient client = new HttpClient();
            var appData = GetClientId();
            var plainTextBytes = Encoding.UTF8.GetBytes(appData.ClientId + ":" + appData.Code);
            string svcCredentials= Convert.ToBase64String(plainTextBytes);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("grant_type", "authorization_code");
            data.Add("code", code);
            data.Add("redirect_uri", "https://localhost:7150/auth/id");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);
            var req = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token") { Content = new FormUrlEncodedContent(data) };
            var response = await client.SendAsync(req) ;

            var contents = await response.Content.ReadAsStreamAsync();

            var token = await JsonSerializer.DeserializeAsync<AuthData>(contents);

            return token;
        }

        private string GetHash(string text)
        {
            StringBuilder Sb = new StringBuilder();

            using SHA256 hash = SHA256Managed.Create();
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(text));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));

            return Sb.ToString();
        }
        private AppData? GetClientId()
        {
            var data = _context.AppData.FirstOrDefault();
            return data;

        }
    }
}
