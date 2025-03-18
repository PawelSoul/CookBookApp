using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CookBookApp.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        // FK do przepisu
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
