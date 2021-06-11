using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using System.Diagnostics;

namespace SamuraiApp.Tests
{
    public class DatabaseTests
    {
        [Fact]
        public void CanInsertSamuraiIntoDatabase()
        {
            using var context = new SamuraiContext(new DbContextOptions<SamuraiContext>());
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var samurai = new Samurai() { Name = "Test" };
            context.Samurais.Add(samurai);
            Debug.WriteLine($"[SQL Server] {samurai.Id} before SaveChanges");
            context.SaveChanges();
            Debug.WriteLine($"[SQL Server] {samurai.Id} after SaveChanges");

            Assert.NotEqual(0, samurai.Id);
        }
    }
}
