﻿using Microsoft.AspNetCore.Identity;

namespace OurRecipes.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
        }
        //recipes created by the user
        public ICollection<Recipe> MyRecipes { get; set; }
        //recipes liked by the user
        public ICollection<Recipe> FavouruteRecipes { get; set; }

    }
}
