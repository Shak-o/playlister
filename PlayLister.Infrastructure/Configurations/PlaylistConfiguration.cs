using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayLister.Infrastructure.Models;

namespace PlayLister.Infrastructure.Configurations
{
    public class PlaylistConfiguration : IEntityTypeConfiguration<PlaylistRepoModel>
    {
        public void Configure(EntityTypeBuilder<PlaylistRepoModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany<ItemRepoModel>(x => x.Items).WithOne(x => x.Playlist).HasForeignKey(x => x.PlaylistId);
        }
    }
}
