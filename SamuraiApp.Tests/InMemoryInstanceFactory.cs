using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;

namespace SamuraiApp.Tests
{
    public class InMemoryInstanceFactory
    {
        public SamuraiContext CreateNewInstance(string instanceName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SamuraiContext>();
            optionsBuilder.UseInMemoryDatabase(instanceName);
            return new SamuraiContext(optionsBuilder.Options);
        }
    }
}