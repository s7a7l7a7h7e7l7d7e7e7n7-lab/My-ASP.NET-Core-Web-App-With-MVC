using Microsoft.EntityFrameworkCore;
using testMVC.Models;

namespace testMVC.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

                
        }

        

    }
}
