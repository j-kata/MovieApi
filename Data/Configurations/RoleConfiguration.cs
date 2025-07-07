using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApi.Models.Entities;

namespace MovieApi.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(cm => new { cm.ActorId, cm.MovieId });

            builder.Property(cm => cm.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(cm => cm.Actor)
                .WithMany(cm => cm.Roles)
                .HasForeignKey(cm => cm.ActorId);

            builder.HasOne(cm => cm.Movie)
                .WithMany(m => m.Roles)
                .HasForeignKey(cm => cm.MovieId);
        }
    }
}