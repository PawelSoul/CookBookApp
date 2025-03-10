using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CookBookApp.Models
{
    internal class Ingredients
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
