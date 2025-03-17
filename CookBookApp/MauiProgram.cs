using CookBookApp.Data;
using CookBookApp.Repositories;
using CookBookApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CookBookApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Wczytanie konfiguracji z appsettings.json
            var config = builder.Configuration;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Pobranie connection string
            var connectionString = config.GetConnectionString("DefaultConnection");

            // Rejestracja DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Rejestracja repozytorium
            builder.Services.AddScoped<IBaseRepository, RecipeRepository>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // --- KLUCZOWA ZMIANA --- tylko jedno builder.Build()
            var app = builder.Build();

            // Automatyczne migracje (przy starcie aplikacji)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }

            return app; // Zwracamy już zbudowaną app
        }
    }
}
