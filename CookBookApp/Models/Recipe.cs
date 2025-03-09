using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CookBookApp.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        public string RecipeName { get; set; }

        // Składniki jako słownik (nazwa składnika -> ilość w gramach)
        [Column(TypeName = "nvarchar(max)")]
        public Dictionary<string, double> Ingredients { get; set; } = new Dictionary<string, double>();

        // Instrukcje jako lista stringów (kolejne kroki przepisu)
        [Column(TypeName = "nvarchar(max)")]
        public List<string> Instructions { get; set; } = new List<string>();
    }
}
