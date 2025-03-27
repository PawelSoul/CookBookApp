using System.Collections.ObjectModel;
using System.Net.WebSockets;
using CookBookApp.Models;
using CookBookApp.Repositories.Interfaces;
using Microsoft.Maui.Storage;

namespace CookBookApp
{
    public partial class Add_Recipe : ContentPage
    {
        IRecipeRepository _recipeRepository;

        Recipe _newRecipe = new Recipe();

        private string _imageFilePath = null;

        private bool _ingredientsVisible = true;
        private bool stepsVisible = true;

        public Add_Recipe(IRecipeRepository baseRepository)// Konstruktor, który dostaje DbContext z DI
        {
            InitializeComponent();
            _recipeRepository = baseRepository;
        }

        // Dodawanie nowego sk³adnika
        private void OnAddIngredientClicked(object sender, EventArgs e)
        {
            var row = new HorizontalStackLayout { Spacing = 15 };

            var removeButton = new Button
            {
                Text = "-",
                Style = (Style)Resources["MinusButtonStyle"]
            };
            removeButton.Clicked += OnRemoveIngredientClicked;

            var ingredientEntry = new Entry
            {
                Placeholder = "Dodaj sk³adnik ...",
                MaxLength = 400,
                BackgroundColor = Color.FromArgb("#3B3533"),
                PlaceholderColor = Colors.White,
                WidthRequest = 425
            };

            var amountEntry = new Entry
            {
                Placeholder = "0.00",
                Keyboard = Keyboard.Numeric,
                BackgroundColor = Color.FromArgb("#3B3533"),
                PlaceholderColor = Colors.White,
                WidthRequest = 80
            };

            var unitPicker = new Picker
            {
                WidthRequest = 110,
                BackgroundColor = Color.FromArgb("#3B3533"),
                TextColor = Colors.White
            };
            unitPicker.ItemsSource = new List<string> { "g", "ml", "szt", "³y¿ka", "szklanka" };
            unitPicker.SelectedIndex = 0;

            row.Children.Add(removeButton);
            row.Children.Add(new Frame { BorderColor = Colors.White, CornerRadius = 5, Padding = 5, BackgroundColor = Colors.Transparent, Content = ingredientEntry });
            row.Children.Add(new Frame { BorderColor = Colors.White, CornerRadius = 5, Padding = 5, BackgroundColor = Colors.Transparent, Content = amountEntry });
            row.Children.Add(new Frame { BorderColor = Colors.White, CornerRadius = 5, Padding = 5, BackgroundColor = Colors.Transparent, Content = unitPicker });

            IngredientsList.Children.Add(row);
        }

        private async void OnIngredientsLabelTapped(object sender, EventArgs e)
        {
            _ingredientsVisible = !_ingredientsVisible;

            if (_ingredientsVisible)
            {
                await ToggleIcon.RotateTo(0, 500, Easing.CubicInOut);

                // Przywracamy ramkê do normalnego wygl¹du
                IngredientsHeaderFrame.BorderColor = Colors.Transparent;

                // Pocz¹tkowy stan do animacji
                IngredientsScrollView.TranslationY = -20;
                AddIngredientsButton.TranslationY = -20;

                IngredientsScrollView.Opacity = 0;
                AddIngredientsButton.Opacity = 0;

                // Ustawiamy widocznoœæ
                IngredientsScrollView.IsVisible = true;
                AddIngredientsButton.IsVisible = true;

                // Animujemy wejœcie
                await Task.WhenAll(
                    IngredientsScrollView.TranslateTo(0, 0, 500, Easing.SinOut),
                    IngredientsScrollView.FadeTo(1, 500),
                    AddIngredientsButton.TranslateTo(0, 0, 500, Easing.SinOut),
                    AddIngredientsButton.FadeTo(1, 500)
                );  
            }
            else
            {
                await ToggleIcon.RotateTo(180, 500, Easing.CubicInOut);
                IngredientsHeaderFrame.BorderColor = Colors.White;

                // Animujemy znikniêcie
                await Task.WhenAll(
                    IngredientsScrollView.TranslateTo(0, 0, 500, Easing.SinOut),
                    IngredientsScrollView.FadeTo(1, 500),
                    AddIngredientsButton.TranslateTo(0, 0, 500, Easing.SinOut),
                    AddIngredientsButton.FadeTo(1, 500)
                );

                // Ukrywamy
                IngredientsScrollView.IsVisible = false;
                AddIngredientsButton.IsVisible = false;
            }
        }
        private async void OnStepsLabelTapped(object sender, EventArgs e)
        {
            stepsVisible = !stepsVisible;

            if (stepsVisible)
            {
                await StepsToggleIcon.RotateTo(0, 500, Easing.CubicInOut);
                StepsHeaderFrame.BorderColor = Colors.Transparent;

                StepsScrollView.TranslationY = -20;
                AddStepButton.TranslationY = -20;

                StepsScrollView.Opacity = 0;
                AddStepButton.Opacity = 0;

                StepsScrollView.IsVisible = true;
                AddStepButton.IsVisible = true;

                await Task.WhenAll(
                    StepsScrollView.TranslateTo(0, 0, 500, Easing.SinOut),
                    StepsScrollView.FadeTo(1, 500),
                    AddStepButton.TranslateTo(0, 0, 500, Easing.SinOut),
                    AddStepButton.FadeTo(1, 500)
                );
            }
            else
            {
                
                await StepsToggleIcon.RotateTo(180, 500, Easing.CubicInOut);
                StepsHeaderFrame.BorderColor = Colors.White;

                // Animujemy znikniêcie
                await Task.WhenAll(
                    StepsScrollView.FadeTo(0, 500),
                    StepsScrollView.TranslateTo(0, -20, 500, Easing.SinIn),
                    AddStepButton.FadeTo(0, 500),
                    AddStepButton.TranslateTo(0, -20, 500, Easing.SinIn)
                );
                // Ukrywamy
                StepsScrollView.IsVisible = false;
                AddStepButton.IsVisible = false;
                
            }
        }


        // Usuwanie sk³adnika
        private void OnRemoveIngredientClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var row = button?.Parent as HorizontalStackLayout;
            if (row != null)
            {
                IngredientsList.Children.Remove(row);
            }
        }

        // Dodawanie nowego kroku
        private void OnAddStepsClicked(object sender, EventArgs e)
        {
            var row = new HorizontalStackLayout { Spacing = 15 };

            var removeButton = new Button
            {
                Text = "-",
                Style = (Style)Resources["MinusButtonStyle"]
            };
            removeButton.Clicked += OnRemoveStepClicked;

            var stepEntry = new Entry
            {
                Placeholder = "Dodaj krok ...",
                FontAttributes = FontAttributes.Italic,
                MaxLength = 500,
                BackgroundColor = Color.FromArgb("#3B3533"),
                PlaceholderColor = Colors.White,
                WidthRequest = 670
            };

            row.Children.Add(removeButton);
            row.Children.Add(new Frame { BorderColor = Colors.White, CornerRadius = 5, Padding = 5, BackgroundColor = Colors.Transparent, Content = stepEntry });

            StepsList.Children.Add(row);
        }

        // Usuwanie kroku
        private void OnRemoveStepClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var row = button?.Parent as HorizontalStackLayout;
            if (row != null)
            {
                StepsList.Children.Remove(row);
            }
        }
        private async void OnAddImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Wybierz zdjêcie",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    _imageFilePath = result.FullPath;
                    RecipeImage.Source = ImageSource.FromFile(_imageFilePath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("B³¹d", $"Nie uda³o siê za³adowaæ zdjêcia: {ex.Message}", "OK");
            }
        }

        private bool ValidationAddAllIngredient()
        {
            bool isValid = true;

            foreach (var ingredient in IngredientsList.Children)
            {
                if (ingredient is HorizontalStackLayout row)
                {
                    Frame? ingredientFrame, amountFrame, pickerFrame;
                    string? ingredientName, amountValue, selectedUnit;
                    GetIngredientDateFromEntry(row, out ingredientFrame, out ingredientName, out amountFrame, out amountValue, out pickerFrame, out selectedUnit);

                    // Walidacja nazwy sk³adnika
                    isValid = ValidateName(isValid, ingredientFrame, ingredientName);

                    // Walidacja iloœci
                    isValid = ValidateAmount(isValid, amountFrame, amountValue);

                    // Walidacja jednostki
                    isValid = ValidateUnit(isValid, pickerFrame, selectedUnit);
                }
            }

            return isValid;
        }

        private static bool ValidateUnit(bool isValid, Frame? pickerFrame, string? selectedUnit)
        {
            if (string.IsNullOrWhiteSpace(selectedUnit))
            {
                pickerFrame.BorderColor = Colors.Red;
                isValid = false;
            }
            else
            {
                pickerFrame.BorderColor = Colors.Transparent;
            }

            return isValid;
        }

        private static bool ValidateAmount(bool isValid, Frame? amountFrame, string? amountValue)
        {
            if (string.IsNullOrWhiteSpace(amountValue) || !double.TryParse(amountValue, out double quantity) || quantity <= 0.00)
            {
                amountFrame.BorderColor = Colors.Red;
                isValid = false;
            }
            else
            {
                amountFrame.BorderColor = Colors.Transparent;
            }

            return isValid;
        }

        private static bool ValidateName(bool isValid, Frame? ingredientFrame, string? ingredientName)
        {
            if (string.IsNullOrWhiteSpace(ingredientName))
            {
                ingredientFrame.BorderColor = Colors.Red;
                isValid = false;
            }
            else
            {
                ingredientFrame.BorderColor = Colors.Transparent;
            }

            return isValid;
        }

        private static void GetIngredientDateFromEntry(HorizontalStackLayout row, out Frame? ingredientFrame, out string? ingredientName, out Frame? amountFrame, out string? amountValue, out Frame? pickerFrame, out string? selectedUnit)
        {
            // Pobierz Entry z nazw¹ sk³adnika
            ingredientFrame = row.Children[1] as Frame;
            var ingredientEntry = ingredientFrame?.Content as Entry;
            ingredientName = ingredientEntry?.Text;

            // Pobierz Entry z iloœci¹
            amountFrame = row.Children[2] as Frame;
            var amountEntry = amountFrame?.Content as Entry;
            amountValue = amountEntry?.Text;

            // Pobierz Picker z jednostk¹
            pickerFrame = row.Children[3] as Frame;
            var unitPicker = pickerFrame?.Content as Picker;
            selectedUnit = unitPicker?.SelectedItem?.ToString();
        }

        private bool ValidAddAllStep()
        {
            bool isValid = true;

            foreach (var step in StepsList.Children)
            {
                if (step is HorizontalStackLayout row)
                {
                    foreach (var child in row.Children)
                    {
                        if (child is Frame frame && frame.Content is Entry entry)
                        {
                            if (string.IsNullOrWhiteSpace(entry.Text))
                            {
                                frame.BorderColor = Colors.Red;
                                isValid = false;
                            }
                            else
                            {
                                frame.BorderColor = Colors.Transparent;
                            }
                        }
                    }
                }
            }

            return isValid;
        }

        private bool ValidateAllCategories()
        {
            bool isValid = true;

            // Przyk³ad dla Twoich pickerów, musisz podaæ ich nazwy tak jak w XAML
            var categoryPickers = new List<Picker> { DishTypePickerRodzajDania, CuisinePickerKuchniaSwiata, MainIngredientPickerGlownySkladnik, OccasionPickerOkazja, OccasionPickerPoziomTrudnosci };

            foreach (var picker in categoryPickers)
            {
                if (picker.SelectedIndex == -1 || picker.SelectedItem == null)
                {
                    picker.TitleColor = Colors.Red;
                    isValid = false;
                }
                else
                {
                    picker.TitleColor = Color.FromArgb("#3B3533"); // lub inny domyœlny kolor
                }
            }

            return isValid;
        }

        //Zapis Przepisu
        private async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            if (!ValidateAll()) return;

            int stepCount = 0;
            var newRecipe = new Recipe
            {
                RecipeName = RecipeNameEntry.Text,
                ImageUrl = _imageFilePath,
                Ingredients = new List<Ingredient>(),
                InstructionSteps = new List<InstructionStep>(),
                CreatedDate = DateTime.Now // Mo¿esz ustawiæ inne wartoœci domyœlne
            };

            // Dodanie sk³adników
            foreach (var ingredient in IngredientsList.Children)
            {
                if (ingredient is HorizontalStackLayout row)
                {
                    Frame? ingredientFrame, amountFrame, pickerFrame;
                    string? ingredientName, amountValue, selectedUnit;
                    GetIngredientDateFromEntry(row, out ingredientFrame, out ingredientName, out amountFrame, out amountValue, out pickerFrame, out selectedUnit);

                    if (!string.IsNullOrWhiteSpace(ingredientName) && double.TryParse(amountValue, out double quantity) && !string.IsNullOrWhiteSpace(selectedUnit))
                    {
                        var newIngredient = new Ingredient
                        {
                            Name = ingredientName,
                            Quantity = quantity,
                            Unit = selectedUnit
                        };

                        newRecipe.Ingredients.Add(newIngredient);
                    }
                }
            }

            // Dodanie kroków
            foreach (var step in StepsList.Children)
            {
                if (step is HorizontalStackLayout row)
                {
                    var stepFrame = row.Children[1] as Frame;
                    var stepEntry = stepFrame?.Content as Entry;
                    var stepDescription = stepEntry?.Text;

                    if (!string.IsNullOrWhiteSpace(stepDescription))
                    {
                        stepCount++;

                        var newInstructionStep = new InstructionStep
                        {
                            StepNumber = stepCount,
                            Description = stepDescription
                        };

                        newRecipe.InstructionSteps.Add(newInstructionStep);
                    }
                }
            }




            // Zapis do bazy danych
            var result = await _recipeRepository.SaveRecipeAsync(newRecipe);

            if (result)
            {
                await DisplayAlert("Sukces", "Przepis zapisany!", "OK");
                // Opcjonalnie: wyczyœæ formularz
            }
            else
            {
                await DisplayAlert("B³¹d", "Wyst¹pi³ problem podczas zapisu.", "OK");
            }
        }

        private bool ValidateAll()
        {
            bool isStepsValid = ValidAddAllStep();
            bool isIngredientsValid = ValidationAddAllIngredient();
            bool isCategoriesValid = ValidateAllCategories();

            return isStepsValid && isIngredientsValid && isCategoriesValid && isCategoriesValid;
        }
    }
}
