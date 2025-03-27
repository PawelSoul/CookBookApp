using CookBookApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CookBookApp.Migrations
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            string dbPath = Path.Combine("D:\\Projects\\CookBookApp", "CookBook.db"); // <- ścieżka do bazy
            optionsBuilder.UseSqlite($"Data Source={dbPath}",
                b => b.MigrationsAssembly("CookBookApp.Migrations"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
