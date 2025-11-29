using GrisAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrisAPI.Configurations;

public sealed class DeckConfiguration : IEntityTypeConfiguration<Deck>
{
    public void Configure(EntityTypeBuilder<Deck> builder)
    {
        builder
            .HasMany(x => x.Jokers)
            .WithMany(x => x.Decks);
        
        builder
            .HasMany(x => x.Cards)
            .WithMany(x => x.Decks);
    }
}