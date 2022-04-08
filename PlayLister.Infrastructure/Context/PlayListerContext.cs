using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayLister.Infrastructure.Models;

namespace PlayLister.Infrastructure.Context
{
    public class PlayListerContext : DbContext
    {
        private readonly string _connectionString;

        public PlayListerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PlayListerContext(DbContextOptions<PlayListerContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.ApplyConfigurationsFromAssembly(typeof(PlayListerContext).Assembly);
        }

        public DbSet<AppData> AppData { get; set; }
        public DbSet<PlaylistRepoModel> Playlists { get; set; } 
        public DbSet<ItemRepoModel> Items { get; set; }
    }
}
