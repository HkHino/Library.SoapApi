using Library.SoapApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.SoapApi.db
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Author> Authors { get; set; }
    }
}
