using GrisAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GrisAPI.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    
    public DbSet<Creature> Creatures { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<ExtraDeck> ExtraDecks { get; set; }
    public DbSet<Joker> Jokers { get; set; }
    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}