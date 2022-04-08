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
    public class ItemConfiguration : IEntityTypeConfiguration<ItemRepoModel>
    {
        public void Configure(EntityTypeBuilder<ItemRepoModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description);
            builder.Property(x => x.Height);
            builder.Property(x => x.Width);
            builder.Property(x => x.Title);
            builder.Property(x => x.Url);

            builder.HasOne<PlaylistRepoModel>(x => x.Playlist).WithMany(x => x.Items);
        }
    }
}
