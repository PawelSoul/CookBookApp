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
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string RecipeName { get; set; } // Nazwa przepisu
        public string Description { get; set; } // Krótki opis
        public int PrepTimeMinutes { get; set; } // Czas przygotowania
        public int CookTimeMinutes { get; set; } // Czas gotowania
        public int Servings { get; set; } // Ilość porcji
        public DifficultyLevel Difficulty { get; set; } // Enum: Easy, Medium, Hard
        public List<Ingredient> Ingredients { get; set; } = new();
        public List<InstructionStep> InstructionSteps { get; set; } = new();
        public List<string> Tags { get; set; } = new(); // Np. "wegetariańskie", "bezglutenowe"
        public string ImageUrl { get; set; } // Link lub ścieżka do zdjęcia
        public DateTime CreatedDate { get; set; }
        public int Rating { get; set; } // Opcjonalnie: ocena przepisu (1-5)

        // Możesz dodać: Ulubione, Kategoria, Autor, itp.
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
    }

    public class InstructionStep
    {
        public int StepNumber { get; set; }
        public string Description { get; set; }
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }
}
