using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBookApp.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        public int? AuthorId { get; set; } // Opcjonalne

        public Author? Author { get; set; }

        [Required]
        public string RecipeName { get; set; } = string.Empty; // NOT NULL, wymagane

        public string? Description { get; set; } // Opcjonalny opis

        public int? PrepTimeMinutes { get; set; } // Opcjonalnie

        public int? CookTimeMinutes { get; set; } // Opcjonalnie

        public int? Servings { get; set; } // Opcjonalnie

        public DifficultyLevel? Difficulty { get; set; } // Opcjonalnie

        // Relacja do Ingredients
        public List<Ingredient> Ingredients { get; set; } = new();

        // Relacja do InstructionSteps
        public List<InstructionStep> InstructionSteps { get; set; } = new();

        public List<string>? Tags { get; set; } = new(); // Opcjonalne (można później przemyśleć relację tabeli Tags)

        public string? ImageUrl { get; set; } // Opcjonalny link do zdjęcia

        public DateTime? CreatedDate { get; set; } // Opcjonalna data

        public int? Rating { get; set; } // Opcjonalna ocena
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }
}
