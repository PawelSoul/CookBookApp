using System;
using System.Collections.Generic;
using System.Linq;
using CookBookApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;

namespace CookBookApp
{
    public partial class MainPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        public MainPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
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
            await Navigation.PushAsync(new Add_Recipe(_dbContext));
        }
    }
}
