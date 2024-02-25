using FormAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FormAPI.Data
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options): base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> customers { get; set; }
        
    }
}
