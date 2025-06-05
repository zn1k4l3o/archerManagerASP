using ArcherManager.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArcherManager.DAL
{
    public class ArcherManagerDbContext : IdentityDbContext<AppUser>
    {
        public ArcherManagerDbContext(DbContextOptions<ArcherManagerDbContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Archer> Archers { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Club> Clubs { get; set; }


    }
}
