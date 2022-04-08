using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlayLister.Infrastructure.Repositories.Implementation;
using PlayLister.Infrastructure.Repositories.Interfaces;

namespace PlayLister.Infrastructure.Repositories.Extensions
{
    public static class RepositoriesExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAppDataRepository, AppDataRepository>();
            services.AddTransient<IPlaylistRepository, PlaylistRepository>();
        }
    }
}
