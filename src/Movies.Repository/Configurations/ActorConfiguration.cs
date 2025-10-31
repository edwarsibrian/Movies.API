using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Domain.Entities;

namespace Movies.Repository.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(a=> a.Name)
                   .IsRequired()
                   .HasMaxLength(150);
            builder.Property(a => a.Picture)
                .IsUnicode(false);
        }
    }
}
