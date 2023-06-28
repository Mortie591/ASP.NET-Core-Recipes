using OurRecipes.Data;
using OurRecipes.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace DataSeeder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetService<DbContext>();
            //Uncomment to import data
            var dataImportService = serviceProvider.GetService<IDataImportService>();
            //dataImportService.CleanDatabase();
            //dataImportService.ImportRecipes();
            var scraperService = serviceProvider.GetService<IScraperService>();
            scraperService.PopulateData();
            Console.WriteLine("Done");
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddTransient<IDataImportService, DataImportService>();
            services.AddTransient<IScraperService, ScraperService>();
        }
    }
}