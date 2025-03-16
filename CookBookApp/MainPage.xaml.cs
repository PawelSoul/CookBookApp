using System;
using System.Collections.Generic;
using System.Linq;
using CookBookApp.Data;
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
            // Pobranie listy zaznaczonych filtrów (przykładowa implementacja)
            var selectedFilters = new List<string>();

            foreach (var child in GetAllCheckBoxes(this))
            {
                if (child.IsChecked)
                {
                    selectedFilters.Add(child.AutomationId ?? "Nieznany Filtr");
                }
            }

            // Tutaj można dodać logikę wyszukiwania na podstawie `selectedFilters`
            Console.WriteLine("Zastosowane filtry: " + string.Join(", ", selectedFilters));
        }

        // Metoda pomocnicza do pobrania wszystkich CheckBox na stronie
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
            string recipeName = RecipeNameEntry.Text;
            if (string.IsNullOrWhiteSpace(recipeName))
            {
                await DisplayAlert("Błąd", "Wpisz nazwę przepisu do wyszukania.", "OK");
                return;
            }

            var foundRecipes = await _IBaseRepository.FindRecipes(recipeName);

            if (foundRecipes.Any())
            {
                string result = string.Join("\n", foundRecipes.Select(r => r.RecipeName));
                await DisplayAlert("Znalezione przepisy", result, "OK");
            }
            else
            {
                await DisplayAlert("Brak wyników", "Nie znaleziono przepisu o podanej nazwie.", "OK");
            }
        }
    }
}
