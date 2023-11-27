using Microsoft.EntityFrameworkCore;
using TextItemManager.Models;

namespace TextItemManager.Data
{
    public class DataContext : DbContext
    {
        public DbSet<TextItem> TextItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=1234");
        }
    }
}
