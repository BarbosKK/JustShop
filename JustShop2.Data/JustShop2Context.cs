using Microsoft.EntityFrameworkCore;
using JustShop2.Core.Domain;

namespace JustShop2.Data
{
    public class JustShop2Context : DbContext
    {
        public JustShop2Context(DbContextOptions<JustShop2Context> options)
        : base(options) { }

        public DbSet<Spaceship> Spaceships { get; set; }
    }
}
