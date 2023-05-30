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

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<UserFavourite> UserFavourites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<AppIdentityUser>()
                .HasMany(x => x.MyRecipes)
                .WithOne(x => x.Author)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ingredient>().HasIndex(x => new { x.Name }).IsUnique();
            builder.Entity<Recipe>().HasKey(x => x.Id);
            
            base.OnModelCreating(builder);
        }
    }
}