using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OurRecipes.Data.Models;

namespace OurRecipes.Data
{
    public class ApplicationDbContext : IdentityDbContext <AppIdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppIdentityUser>().HasMany(x => x.FavouruteRecipes).WithMany(x => x.LikedBy);
            builder.Entity<AppIdentityUser>().HasMany(x => x.MyRecipes).WithOne(x => x.Author);
            builder.Entity<Recipe>().HasKey(x => x.Id);
            base.OnModelCreating(builder);
        }
    }
}