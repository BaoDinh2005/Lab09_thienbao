using Lab09_thienbao.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab09_thienbao.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}