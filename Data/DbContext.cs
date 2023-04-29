using Microsoft.EntityFrameworkCore;
using ParseRSS.Models;

namespace ParseRSS.Data
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options)
        {
        }

        public DbSet<News> News { get; set; }
    }
}
