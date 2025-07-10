using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.Models.Entities;

namespace MovieAPI.Data.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.ToTable("Actors");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name) // can have same name?
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.BirthYear)
                .IsRequired();
        }
    }
}