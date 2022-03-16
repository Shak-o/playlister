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
    public class AppDataConfiguration : IEntityTypeConfiguration<AppData>
    {
        public void Configure(EntityTypeBuilder<AppData> builder)
        {
            builder.HasNoKey();
            builder.Property(x => x.ClientId);
            builder.Property(x => x.Code);
        }
    }
}
