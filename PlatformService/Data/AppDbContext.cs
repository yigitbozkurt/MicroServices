using Microsoft.EntityFrameworkCore;
using PlatformService.Models;   

namespace PlatformService.Data{

    public class AppDbContext : DbContext{

        #pragma warning disable CS8618
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Platform> Platforms {get; set;}

    }
}   