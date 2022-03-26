using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlayLister.Services.Implementation;
using PlayLister.Services.Interfaces;

namespace PlayLister.Services.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection service)
        {
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<IPlaylistConverter, PlaylistConverter>();
        }
    }
}
