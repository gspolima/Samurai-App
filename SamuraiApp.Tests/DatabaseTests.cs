using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
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
            Debug.WriteLine($"{samurai.Id} before SaveChanges");
            context.SaveChanges();
            Debug.WriteLine($"{samurai.Id} after SaveChanges");

            Assert.NotEqual(0, samurai.Id);
        }
    }
}
