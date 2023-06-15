// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Identity;
using OurRecipes.Data;
using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace OurRecipes.Areas.Identity.Pages.Account
{
    internal class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            ApplicationDbContext context = (ApplicationDbContext)validationContext
                         .GetService(typeof(ApplicationDbContext));
            var user = context.Users.FirstOrDefault(x => x.UserName == value.ToString());
            if(user == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Username \'{value}\' already exists.");
            }
        }
    }
}