using CookBookApp.Data;
using Microsoft.EntityFrameworkCore;
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

            // Pobranie connection stringa (możesz wczytać z konfiguracji zamiast hardcodować)
            string connectionString = "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=CookBookDB;Persist Security Info=False;User ID=etoe_pawel;Password=Gwiazda100;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // Dodanie DbContext do DI
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
