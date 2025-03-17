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
                optionsBuilder.UseSqlServer("Server=tcp:etoe-database-server.database.windows.net,1433;Initial Catalog=etoe-database;Persist Security Info=False;User ID=etoe_pawel;Password=Gwiazda100;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
                    b => b.MigrationsAssembly("CookBookApp.Migrations"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
