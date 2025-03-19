using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Repositories;
using CookBookApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;

namespace CookBookApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IBaseRepository _IBaseRepository;

        public MainPage(IBaseRepository recipeRepository)
        {
            InitializeComponent();
            _IBaseRepository = recipeRepository;
        }

        // Metoda wywoływana po zmianie stanu CheckBox
        private void OnFilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        // Logika filtrowania przepisów
        private void ApplyFilters()
        {
            var selectedFilters = new List<string>();

            foreach (var child in GetAllCheckBoxes(this))
            {
                if (child.IsChecked)
                {
                    selectedFilters.Add(child.AutomationId ?? "Nieznany Filtr");
                }
            }

            Console.WriteLine("Zastosowane filtry: " + string.Join(", ", selectedFilters));
        }

        // Pobranie wszystkich CheckBox
        private IEnumerable<CheckBox> GetAllCheckBoxes(VisualElement parent)
        {
            if (parent is CheckBox checkBox)
                yield return checkBox;

            if (parent is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    foreach (var cb in GetAllCheckBoxes((VisualElement)child))
                    {
                        yield return cb;
                    }
                }
            }
        }

        private async void OnAddRecipeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Add_Recipe(_IBaseRepository));
        }

        private async void OnSearchRecipeClicked(object sender, EventArgs e)
        {
            string searchText = RecipeNameEntry.Text?.ToLower();

            SearchResultsLayout.Children.Clear();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                SearchResultsLayout.Children.Add(new Label { Text = "Wpisz nazwę przepisu.", TextColor = Colors.Red });
                return;
            }

            var foundRecipes = await _IBaseRepository.FindRecipes(searchText);

            if (!foundRecipes.Any())
            {
                SearchResultsLayout.Children.Add(new Label { Text = "Nie znaleziono przepisów.", TextColor = Colors.Gray });
                return;
            }

            foreach (var recipe in foundRecipes)
            {
                SearchResultsLayout.Children.Add(new Label
                {
                    Text = recipe.RecipeName,
                    TextColor = Colors.Black,
                    FontSize = 16
                });
            }
        }
    }
}
