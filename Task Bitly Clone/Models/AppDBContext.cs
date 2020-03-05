using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_Bitly_Clone.Models
{
    public class AppDBContext : DbContext
    {
        public DbSet<ShortUrls> shortUrl { get; set; }
        public DbSet<Tracks> tracks { get; set; }
        public DbSet<Users> users { get; set; }
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
    }
}
