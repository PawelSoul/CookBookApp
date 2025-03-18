using Microsoft.EntityFrameworkCore;
using CookBookApp.Models;

namespace CookBookApp.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString = "Server=tcp:etoe-database-server.database.windows.net,1433;Initial Catalog=etoe-database;Persist Security Info=False;User ID=etoe_pawel;Password=Gwiazda100;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False";

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<InstructionStep> InstructionSteps { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("CookBookApp.Migrations"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji Recipe - Ingredient
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji Recipe - InstructionStep
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.InstructionSteps)
                .WithOne(s => s.Recipe)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji Author - Recipe
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Recipes)
                .WithOne(r => r.Author)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Możesz ustawić inne ograniczenia, np. dla Tags, ale to opcjonalnie
        }
    }
}
