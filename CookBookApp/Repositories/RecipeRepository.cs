using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace CookBookApp.Repositories
{
    internal class RecipeRepository : IBaseRepository
    {
        private readonly ILogger<RecipeRepository> _logger;
        private readonly AppDbContext _dbContext;

        public RecipeRepository(AppDbContext dbContext, ILogger<RecipeRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<List<Recipe>> FindRecipes(string name)
        {
            return await _dbContext.Recipes
                .Where(r => EF.Functions.Like(r.RecipeName, $"%{name}%"))
                .ToListAsync();
        }

        public async Task<bool> SaveRecipeAsync(Recipe recipe)
        {
            try
            {
                await _dbContext.Recipes.AddAsync(recipe);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding recipe");
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

        public async Task<Recipe?> GetRecipeByIdAsync(int recipeId)
        {
            return await _dbContext.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.InstructionSteps)
                .FirstOrDefaultAsync(r => r.Id == recipeId);
        }
    }
}
