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

        public async Task<List<Recipe>> GetAllRecipesWithAuthorAsync()
        {
            var recipesWithAuthors = await _dbContext.Recipes
                .Include(r => r.Author) // ładuje dane autora
                .ToListAsync();
                
            //foreach (var recipe in recipesWithAuthors)
            //{
            //    Console.WriteLine($"Przepis: {recipe.RecipeName}, Autor: {recipe.Author.Name}");
            //}
            return recipesWithAuthors;
        }

        public async Task<List<Author>> GetAllAuthorsWithRecipeAsync()
        {
            var authorsWithRecipes = await _dbContext.Authors
                .Include(a => a.Recipes)
                .ToListAsync();

            //foreach (var author in authorsWithRecipes)
            //{
            //    Console.WriteLine($"Autor: {author.Name}");
            //    foreach (var recipe in author.Recipes)
            //    {
            //        Console.WriteLine($"  - Przepis: {recipe.RecipeName}");
            //    }
            //}
            return authorsWithRecipes;
        }

        public async Task<List<Recipe>> GetRecipesFromAuthorAsync(int autorID)
        {
            int authorId = autorID; // Przykładowy ID autora

            var recipes = await _dbContext.Recipes
                .Where(r => r.AuthorId == authorId)
                .ToListAsync();

            foreach (var recipe in recipes)
            {
                Console.WriteLine($"Przepis: {recipe.RecipeName}");
            }
            return recipes;
        }

        public async Task<Recipe> GetRecipeAsync(int recipeID)
        {
            int recipeId = recipeID;

            var recipe = await _dbContext.Recipes
                .Include(r => r.Author)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            //if (recipe != null)
            //{
            //    Console.WriteLine($"Przepis: {recipe.RecipeName}, Autor: {recipe.Author.Name}");
            //}
            return recipe;
        }
    }
}
