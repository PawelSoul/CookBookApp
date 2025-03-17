using CookBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookApp.Repositories.Interfaces
{
    public interface IBaseRepository
    {
        Task<List<Recipe>> FindRecipes(string name);
        Task<bool> SaveRecipeAsync(Recipe recipe);
        Task<List<Recipe>> GetAllRecipesWithAuthorAsync();
    }
}
