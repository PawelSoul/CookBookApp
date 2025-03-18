using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBookApp.Models
{
    public class InstructionStep
    {
        [Key]
        public int Id { get; set; }

        public int StepNumber { get; set; }

        public string Description { get; set; } = string.Empty;

        // FK do przepisu
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
