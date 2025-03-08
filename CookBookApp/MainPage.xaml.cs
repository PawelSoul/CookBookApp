using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace CookBookApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
    }
}
