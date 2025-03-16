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
    }
}
