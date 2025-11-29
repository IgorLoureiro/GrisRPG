using System.Diagnostics.CodeAnalysis;
using GrisAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrisAPI.Configurations;

[ExcludeFromCodeCoverage]
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany(x => x.Creatures)
            .WithMany(x => x.Users);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}