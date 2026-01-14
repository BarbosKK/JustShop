using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JustShop2.Core.Domain;
using Microsoft.AspNetCore.Identity;

namespace JustShop2.Data
{
    public class JustShop2Context : DbContext //IdentityDbContext<ApplicationUser>
    {
        public JustShop2Context(DbContextOptions<JustShop2Context> options)
        : base(options) { }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<FileToDatabase> FileToDatabases { get; set; }
        public DbSet<Kindergarten> Kindergartens { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
    }

}