using CookBookApp.Models;
using Microsoft.Maui.Controls;

namespace CookBookApp
{
    public partial class RecipeDetailPage : ContentPage
    {
        public RecipeDetailPage(Recipe recipe)
        {
            //InitializeComponent();

            BindingContext = recipe;

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 20,
                    Children =
                    { 
                        new Label
                        {
                            Text = recipe.RecipeName,
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 24,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        new Label
                        {
                            Text = $"Autor: {recipe.Author}",
                            FontSize = 14,
                            TextColor = Colors.Gray,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        new Image
                        {
                            Source = !string.IsNullOrEmpty(recipe.ImageUrl) ? recipe.ImageUrl : "default_recipe.png",
                            Aspect = Aspect.AspectFill,
                            HeightRequest = 200,
                            WidthRequest = 300,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        new Label
                        {
                            Text = "Opis:",
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 18
                        },
                        new Label
                        {
                            Text = recipe.Description ?? "Brak opisu.",
                            FontSize = 16
                        },
                        new Label
                        {
                            Text = "Sk³adniki:",
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 18
                        },
                        new Label
                        {
                            Text = recipe.Ingredients != null && recipe.Ingredients.Any()
                                ? string.Join(", ", recipe.Ingredients.Select(i => i.Name))
                                : "Brak sk³adników.",
                            FontSize = 16
                        },
                        new Label
                        {
                            Text = "Kroki przygotowania:",
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 18
                        },
                        new Label
                        {
                            Text = recipe.InstructionSteps != null && recipe.InstructionSteps.Any()
                                ? string.Join("\\n", recipe.InstructionSteps.Select(s => s.Description))
                                : "Brak instrukcji.",
                            FontSize = 16
                        }
                    }
                }
            };
        }
    }
}
