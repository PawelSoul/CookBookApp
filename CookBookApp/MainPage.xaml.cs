using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
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
        private readonly IRecipeRepository _IBaseRepository;

        public MainPage(IRecipeRepository recipeRepository)
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
                var recipeCard = new Frame
                {
                    BackgroundColor = Color.FromArgb("#F5F5F5"),
                    CornerRadius = 15,
                    Padding = 10,
                    Margin = new Thickness(5),
                    Content = new VerticalStackLayout
                    {
                        Spacing = 5,
                        Children =
                        {
                            new Label
                            {
                                Text = recipe.RecipeName,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 18,
                                TextColor = Colors.Black
                            },
                            new Label
                            {
                                Text = $"Autor: {recipe.Author}",
                                FontSize = 14,
                                TextColor = Colors.Gray
                            },
                            new Button
                            {
                                Text = "Zobacz przepis",
                                BackgroundColor = Color.FromArgb("#A85400"),
                                TextColor = Colors.White,
                                CornerRadius = 10,
                                Command = new Command(async () =>
                                {
                                    await Navigation.PushAsync(new RecipeDetailPage(recipe));
                                })
                            }
                        }
                    }
                };

                SearchResultsLayout.Children.Add(recipeCard);
            }
        }
        private void OnFilterTapped(object sender, EventArgs e)
        {
            if (sender is HorizontalStackLayout layout)
            {
                foreach (var child in layout.Children)
                {
                    if (child is CheckBox checkbox)
                    {
                        checkbox.IsChecked = !checkbox.IsChecked;
                        break;
                    }
                }
            }
        }
        private async void OnHeaderPointerEntered(object sender, PointerEventArgs e)
        {
            if (sender is Label label)
                await label.ScaleTo(1.1, 100); // Powiększenie
        }

        private async void OnHeaderPointerExited(object sender, PointerEventArgs e)
        {
            if (sender is Label label)
                await label.ScaleTo(1.0, 100); // Powrót
        }
        // RODZAJ DANIA
        private async void OnRodzajDaniaPointerEntered(object sender, PointerEventArgs e) => await RodzajDaniaLabel.ScaleTo(1.1, 100);
        private async void OnRodzajDaniaPointerExited(object sender, PointerEventArgs e) => await RodzajDaniaLabel.ScaleTo(1.0, 100);
        private void OnRodzajDaniaExpandedChanged(object sender, EventArgs e) => RodzajDaniaArrow.Text = RodzajDaniaExpander.IsExpanded ? "▲" : "▼";

        // KUCHNIA ŚWIATA
        private async void OnKuchniaPointerEntered(object sender, PointerEventArgs e) => await KuchniaLabel.ScaleTo(1.1, 100);
        private async void OnKuchniaPointerExited(object sender, PointerEventArgs e) => await KuchniaLabel.ScaleTo(1.0, 100);
        private void OnKuchniaExpandedChanged(object sender, EventArgs e) => KuchniaArrow.Text = KuchniaExpander.IsExpanded ? "▲" : "▼";

        // GŁÓWNY SKŁADNIK
        private async void OnSkladnikPointerEntered(object sender, PointerEventArgs e) => await SkladnikLabel.ScaleTo(1.1, 100);
        private async void OnSkladnikPointerExited(object sender, PointerEventArgs e) => await SkladnikLabel.ScaleTo(1.0, 100);
        private void OnSkladnikExpandedChanged(object sender, EventArgs e) => SkladnikArrow.Text = SkladnikExpander.IsExpanded ? "▲" : "▼";

        // OKAZJA
        private async void OnOkazjaPointerEntered(object sender, PointerEventArgs e) => await OkazjaLabel.ScaleTo(1.1, 100);
        private async void OnOkazjaPointerExited(object sender, PointerEventArgs e) => await OkazjaLabel.ScaleTo(1.0, 100);
        private void OnOkazjaExpandedChanged(object sender, EventArgs e) => OkazjaArrow.Text = OkazjaExpander.IsExpanded ? "▲" : "▼";

        // STOPIEŃ TRUDNOŚCI
        private async void OnTrudnoscPointerEntered(object sender, PointerEventArgs e) => await TrudnoscLabel.ScaleTo(1.1, 100);
        private async void OnTrudnoscPointerExited(object sender, PointerEventArgs e) => await TrudnoscLabel.ScaleTo(1.0, 100);
        private void OnTrudnoscExpandedChanged(object sender, EventArgs e) => TrudnoscArrow.Text = TrudnoscExpander.IsExpanded ? "▲" : "▼";


    }
}
