using CookBookApp.Data;
using CookBookApp.Repositories;
using CookBookApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui; 
using System.IO;
using Microsoft.Maui.Storage;

namespace CookBookApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() // Dodaj to!
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Rejestracja DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                string dbPath = Path.Combine("D:\\Projects\\CookBookApp", "CookBook.db");
                options.UseSqlite($"Data Source={dbPath}");
            });

            // Rejestracja repozytorium
            builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Automatyczne migracje (przy starcie aplikacji)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }

            return app;
        }
    }
}
