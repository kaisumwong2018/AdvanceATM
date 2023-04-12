using Microsoft.EntityFrameworkCore;

namespace AdvanceATM.Model
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> customers { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Transaction> transactions { get; set; }    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-M3BPRVG\SQLEXPRESS;Database=ATM;Trusted_Connection=True;");
        }
    }
}
