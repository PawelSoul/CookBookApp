using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using CookBookApp.Models;
using CookBookApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CookBookApp
{
    public partial class Add_Recipe : ContentPage
    {
        public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>();
        private readonly AppDbContext _dbContext; // Pole na DbContext

        public Add_Recipe(AppDbContext dbContext)// Konstruktor, kt�ry dostaje DbContext z DI
        {
            InitializeComponent();
            ImagesCollectionView.ItemsSource = Images;
            _dbContext = dbContext;
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
                await DisplayAlert("B��d", $"Nie uda�o si� doda� zdj�cia: {ex.Message}", "OK");
            }
        }

        private async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            string recipeName = RecipeNameEntry.Text;
            string ingredientsText = IngredientsEditor.Text;
            string stepsText = StepsEditor.Text;

            // Sprawdzenie, czy pola nie s� puste
            if (string.IsNullOrWhiteSpace(recipeName) || string.IsNullOrWhiteSpace(ingredientsText) || string.IsNullOrWhiteSpace(stepsText))
            {
                await DisplayAlert("B��d", "Prosz� uzupe�ni� wszystkie pola!", "OK");
                return;
            }

            var ingredientsDict = new Dictionary<string, double>();
            foreach (var item in ingredientsText.Split(','))
            {
                var parts = item.Split(':');
                if (parts.Length == 2 && double.TryParse(parts[1], out double value))
                {
                    ingredientsDict[parts[0].Trim()] = value;
                }
                else
                {
                    await DisplayAlert("B��d", "Niepoprawny format sk�adnik�w! U�yj formatu: nazwa1:100, nazwa2:200", "OK");
                    return;
                }
            }

            var instructionsList = stepsText.Split('.')
                                    .Select(s => s.Trim())
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .ToList();

            var recipe = new Recipe
            {
                RecipeName = recipeName,
                Ingredients = ingredientsDict,
                Instructions = instructionsList
            };

            try
            {
                _dbContext.Recipes.Add(recipe);
                await _dbContext.SaveChangesAsync();

                await DisplayAlert("Sukces", "Przepis zosta� zapisany!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("B��d", $"Nie uda�o si� zapisa� przepisu: {ex.Message}", "OK");
            }
        }

    }
}
