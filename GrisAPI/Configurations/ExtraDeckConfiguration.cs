using GrisAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrisAPI.Configurations;

public sealed class ExtraDeckConfiguration : IEntityTypeConfiguration<ExtraDeck>
{
    public void Configure(EntityTypeBuilder<ExtraDeck> builder)
    {
        builder
            .HasMany(x => x.Jokers)
            .WithMany(x => x.ExtraDecks);
        
        builder
            .HasMany(x => x.Cards)
            .WithMany(x => x.ExtraDecks);
    }
}