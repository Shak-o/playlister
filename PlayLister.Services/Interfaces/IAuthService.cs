using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayLister.Services.Models;
using PlayLister.Services.Models.Spotify;

namespace PlayLister.Services.Interfaces
{
    public interface IAuthService
    {
        string GetUri();
        Task<AuthData> RequestToken(string code);
        Task<RefreshTokenData?> RefreshToken(string token, string refreshToken);
    }
}
