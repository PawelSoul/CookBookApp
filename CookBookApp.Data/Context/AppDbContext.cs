using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CookBookApp.Models;
using System.Text.Json;

namespace CookBookApp.Data 
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString = "Server=tcp:etoe-database-server.database.windows.net,1433;Initial Catalog=etoe-database;Persist Security Info=False;User ID=etoe_pawel;Password=Gwiazda100;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False";
        

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Author> Authors { get; set; }

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
            var ConvertIngredientList = new ValueConverter<List<Ingredient>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<Ingredient>>(v, (JsonSerializerOptions)null) ?? new List<Ingredient>()
            );

            var ConvertInstructionStepList = new ValueConverter<List<InstructionStep>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<InstructionStep>>(v, (JsonSerializerOptions)null) ?? new List<InstructionStep>()
            );

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Ingredients)
                .HasConversion(ConvertIngredientList);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.InstructionSteps)
                .HasConversion(ConvertInstructionStepList);

            // Konfiguracja relacji Author - Recipe
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Recipes)
                .WithOne(r => r.Author)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // możesz ustawić co ma się dziać przy usunięciu autora
        }
    }
}
