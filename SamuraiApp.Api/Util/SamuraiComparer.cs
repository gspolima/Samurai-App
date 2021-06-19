using SamuraiApp.Domain;
using System.Collections.Generic;

namespace SamuraiApp.Api.Util
{
    public class SamuraiComparer : IEqualityComparer<Samurai>
    {
        public bool Equals(Samurai x, Samurai y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Samurai samurai)
        {
            return samurai.Id;
        }
    }
}
