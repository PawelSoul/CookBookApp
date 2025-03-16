using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CookBookApp.Repositories
{
    internal class baseRepository : IBaseRepository
    {
        private readonly AppDbContext _dbContext;

        public baseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Recipe>> FindRecipes(string name)
        {
            return await _dbContext.Recipes
                .Where(r => EF.Functions.Like(r.RecipeName, $"%{name}%"))
                .ToListAsync();
        }

        public async Task<bool> AddRecipeAsync(Recipe recipe)
        {
            try
            {
                await _dbContext.Recipes.AddAsync(recipe);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Możesz logować błąd tutaj
                Console.WriteLine($"Error adding recipe: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            return await _dbContext.Recipes.ToListAsync();
        }
    }
}
