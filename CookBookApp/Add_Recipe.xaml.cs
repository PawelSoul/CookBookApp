using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using CookBookApp.Models;
using CookBookApp.Data;
using Microsoft.EntityFrameworkCore;
using CookBookApp.Repositories.Interfaces;

namespace CookBookApp
{
    public partial class Add_Recipe : ContentPage
    {
        public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>();

        public Dictionary<string, double> Ingredients { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, double?> InstructionSteps { get; set; } = new Dictionary<string, double?>();

        IBaseRepository _baseRepository;

        public Add_Recipe(IBaseRepository baseRepository)// Konstruktor, który dostaje DbContext z DI
        {
            InitializeComponent();
            ImagesCollectionView.ItemsSource = Images;
            _baseRepository = baseRepository;

            var newRecipe = new Recipe();
        }

        private async void OnAddImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    string imagePath = result.FullPath;
                    Images.Add(imagePath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("B³¹d", $"Nie uda³o siê dodaæ zdjêcia: {ex.Message}", "OK");
            }
        }

        private void OnAddIngredientClicked(object sender, EventArgs e)
        {
            AddIngredientEntry("Dodaj sk³adnik", "g");
        }

        private void OnAddStepsClicked(object sender, EventArgs e)
        {
            AddStepEntry("Dodaj krok ...", "min");
        }

        // Funkcja do dodawania nowego sk³adnika z gramatur¹
        private void AddIngredientEntry(string placeholder, string unit)
        {
            var layout = new HorizontalStackLayout { Spacing = 5 };

            var removeButton = new Button
            {
                Text = "-",
                FontSize = 20,
                TextColor = Colors.White,
                BackgroundColor = Colors.Red,
                WidthRequest = 40,
                HeightRequest = 40
            };

            var ingredientEntry = new Entry
            {
                Placeholder = placeholder,
                FontAttributes = FontAttributes.Italic,
                MaxLength = 200,
                BackgroundColor = Color.FromHex("#3B3533"),
                PlaceholderColor = Colors.White,
                WidthRequest = 400
            };

            var amountEntry = new Entry
            {
                Placeholder = unit,
                Keyboard = Keyboard.Numeric,
                BackgroundColor = Color.FromHex("#3B3533"),
                PlaceholderColor = Colors.White,
                WidthRequest = 80
            };

            removeButton.Clicked += (s, e) => IngredientsList.Children.Remove(layout);

            layout.Children.Add(removeButton);
            layout.Children.Add(ingredientEntry);
            layout.Children.Add(amountEntry);

            IngredientsList.Children.Add(layout);
        }

        // Funkcja do dodawania nowego kroku z czasem wykonania
        private void AddStepEntry(string placeholder, string unit)
        {
            var layout = new HorizontalStackLayout { Spacing = 5 };

            var removeButton = new Button
            {
                Text = "-",
                FontSize = 20,
                TextColor = Colors.White,
                BackgroundColor = Colors.Red,
                WidthRequest = 40,
                HeightRequest = 40
            };

            var stepEntry = new Entry
            {
                Placeholder = placeholder,
                FontAttributes = FontAttributes.Italic,
                MaxLength = 200,
                BackgroundColor = Color.FromHex("#3B3533"),
                PlaceholderColor = Colors.White,
                WidthRequest = 400
            };

            var timeEntry = new Entry
            {
                Placeholder = unit,
                Keyboard = Keyboard.Numeric,
                BackgroundColor = Color.FromHex("#3B3533"),
                PlaceholderColor = Colors.White,
                WidthRequest = 80
            };

            removeButton.Clicked += (s, e) => StepsList.Children.Remove(layout);

            layout.Children.Add(removeButton);
            layout.Children.Add(stepEntry);
            layout.Children.Add(timeEntry);

            StepsList.Children.Add(layout);
        }

        // Usuwanie pierwszego sk³adnika
        private void OnRemoveIngredientClicked(object sender, EventArgs e)
        {
            FirstIngredientRow.IsVisible = false;
        }

        // Usuwanie pierwszego kroku
        private void OnRemoveStepClicked(object sender, EventArgs e)
        {
            FirstStepRow.IsVisible = false;
        }

        //Zapis Przepisu
        private async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            var newRecipe = new Recipe
            {
                RecipeName = RecipeNameEntry.Text,
                Ingredients = new Dictionary<string, double>
            {
                { "Makaron", 200 },
                { "Sos pomidorowy", 150 }
            },
                InstructionSteps = new Dictionary<string, double?>
            {
                { "Ugotuj makaron", 10 },
                { "Dodaj sos", 5 }
            }
            };

            var result = await _baseRepository.AddRecipeAsync(newRecipe);

            if (result)
            {
                // Sukces
            }
            else
            {
                // Obs³uga b³êdu
            }
        }


    }
}
