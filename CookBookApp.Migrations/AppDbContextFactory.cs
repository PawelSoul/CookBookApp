using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CookBookApp.Data; // namespace nowego projektu

namespace CookBookApp.Migrations
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("TU_WKLEJ_CONNECTION_STRING");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
