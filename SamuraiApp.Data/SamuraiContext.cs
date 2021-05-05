using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
using System;
using System.IO;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        private StreamWriter _streamWriter =
            new StreamWriter(
                $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Log_SamuraiAppData.txt", append: true);

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Horse> Horses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData")
                .LogTo(_streamWriter.WriteLine, LogLevel.Debug)
                .EnableSensitiveDataLogging();
        }

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
        }
    }
}
