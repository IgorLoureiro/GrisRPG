using GrisAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrisAPI.Configurations;

public sealed class CreatureConfiguration : IEntityTypeConfiguration<Creature>
{
    public void Configure(EntityTypeBuilder<Creature> builder)
    {
        builder
            .HasMany(x => x.Decks)
            .WithMany(x => x.Creatures);

        builder
            .HasOne(x => x.ExtraDeck)
            .WithOne(x => x.Creature)
            .HasForeignKey<Creature>(x => x.ExtraDeckId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}