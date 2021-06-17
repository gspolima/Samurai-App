using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public SamuraiContext()
        { }

        public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options)
        { }

        public SamuraiContext(DbContextOptionsBuilder options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source= (localdb)\\MSSQLLocalDB;Initial Catalog=SamuraiTestData");
            }
        }

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Horse> Horses { get; set; }
        public DbSet<SamuraiBattleStats> SamuraiBattleStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(),
                 bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SamuraiBattleStats>()
                .HasNoKey()
                .ToView("SamuraiBattleStats");

            modelBuilder.Entity<Quote>()
                .Ignore(q => q.SamuraiIdHasValue);
        }
    }
}
