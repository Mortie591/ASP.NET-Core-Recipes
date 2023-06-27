using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Services;
using AutoMapper;
using OurRecipes.Models.Profiles;

namespace OurRecipes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options
               .UseLazyLoadingProxies()
               .UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<AppIdentityUser>(options =>
            {
                //Lower requirements for password management (for easier testing)
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 4;

            }).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            //Custom services registration
            
            builder.Services.AddTransient<IRecipeService, RecipeService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<ICommentService, CommentService>();
            builder.Services.AddAutoMapper(c=>c.AddProfile<RecipeProfile>(),typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}