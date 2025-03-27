using CookBookApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("✅ EF Runner is configured. Use it as --startup-project for migrations.");

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            string dbPath = Path.Combine("D:\\Projects\\CookBookApp", "CookBook.db"); // Zmień ścieżkę jeśli trzeba
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}", b => b.MigrationsAssembly("CookBookApp.Migrations")));
        });
