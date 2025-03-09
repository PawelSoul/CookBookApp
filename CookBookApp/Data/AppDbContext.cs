using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CookBookApp.Models;
using System.Text.Json;

namespace CookBookApp.Data 
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString = "Server=tcp:etoe-database-server.database.windows.net,1433;Initial Catalog=etoe-database;Persist Security Info=False;User ID=etoe_pawel;Password=Gwiazda100;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=\"Active Directory Password";

        public DbSet<Recipe> Recipes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Zamienia biblioteke Dictionary<string, double> na string lub na odwrót
            var ConvertDictionaryString = new ValueConverter<Dictionary<string, double>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, double>>(v, (JsonSerializerOptions)null)
            );
            //Zamienia biblioteke List<string> na string lub na odwrót
            var ConvertListString = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
            );

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Ingredients)
                .HasConversion(ConvertDictionaryString);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Instructions)
                .HasConversion(ConvertListString);
        }
    }
}
