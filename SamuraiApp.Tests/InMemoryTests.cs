using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using System.Diagnostics;

namespace SamuraiApp.Tests
{
    public class InMemoryTests
    {
        [Fact]
        public void CanInsertSamuraiIntoInMemoryDatabase()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanInsertSamurai");
            using var context = new SamuraiContext(builder.Options);

            var samurai = new Samurai() { Name = "Test" };
            context.Samurais.Add(samurai);
            Assert.Equal(EntityState.Added, context.Entry(samurai).State);

            Debug.WriteLine($"[InMemory] {samurai.Id} before SaveChanges");
            context.SaveChanges();
            Debug.WriteLine($"[InMemory] {samurai.Id} after SaveChanges");
        }
    }
}
