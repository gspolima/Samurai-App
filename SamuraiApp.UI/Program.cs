using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    public class Program
    {
        static SamuraiContext _context = new SamuraiContext();
        static SamuraiContextNoTracking _contextNT = new SamuraiContextNoTracking();

        static void Main(string[] args)
        {
            Console.Write("Press Any Key...");
            Console.Read();
        }
    }
}