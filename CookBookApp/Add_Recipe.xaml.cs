using System.Collections.ObjectModel;
using System.Net.WebSockets;
using CookBookApp.Models;
using CookBookApp.Repositories.Interfaces;
using Microsoft.Maui.Storage;

namespace CookBookApp
{
    public partial class Add_Recipe : ContentPage
    {
        IBaseRepository _baseRepository;

        Recipe _newRecipe = new Recipe();

        private string _imageFilePath = null;

        public Add_Recipe(IBaseRepository baseRepository)// Konstruktor, który dostaje DbContext z DI
        {
            InitializeComponent();
            _baseRepository = baseRepository;

            //_newRecipe = new Recipe();
            //_newRecipe.Ingredients = new List<Ingredient>();
            //_newRecipe.InstructionSteps = new List<InstructionStep>();
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

        // Funkcja do dodawania nowego sk³adnika z gramatur¹
        private void AddIngredientEntry(string placeholderIngredient, string placeholderAmount)
        {
            var layout = new HorizontalStackLayout { Spacing = 15 };

            // Przycisk usuwania
            var removeButton = new Button
            {
                Text = "-",
                FontSize = 30,
                TextColor = Colors.White,
                BackgroundColor = Colors.Red,
                WidthRequest = 40,
                HeightRequest = 40
            };

            // Entry: Sk³adnik
            var ingredientEntryFrame = new Frame
            {
                BorderColor = Colors.White,
                CornerRadius = 5,
                Padding = 5,
                BackgroundColor = Colors.Transparent,
                Content = new Entry
                {
                    Placeholder = placeholderIngredient,
                    MaxLength = 400,
                    BackgroundColor = Color.FromHex("#3B3533"),
                    PlaceholderColor = Colors.White,
                    WidthRequest = 425
                }
            };

            // Entry: Iloœæ
            var amountEntryFrame = new Frame
            {
                BorderColor = Colors.White,
                CornerRadius = 5,
                Padding = 5,
                BackgroundColor = Colors.Transparent,
                Content = new Entry
                {
                    Placeholder = placeholderAmount,
                    Keyboard = Keyboard.Numeric,
                    BackgroundColor = Color.FromHex("#3B3533"),
                    PlaceholderColor = Colors.White,
                    WidthRequest = 80
                }
            };

            // Picker: Jednostka
            var unitPicker = new Picker
            {
                WidthRequest = 110,
                BackgroundColor = Color.FromHex("#3B3533"),
                TextColor = Colors.White,
                ItemsSource = new List<string> { "g", "ml", "szt", "³y¿ka", "szklanka" },
                //SelectedIndex = -1 // domyœlnie wybrana jednostka
            };

            var unitPickerFrame = new Frame
            {
                BorderColor = Colors.White,
                CornerRadius = 5,
                Padding = 5,
                BackgroundColor = Colors.Transparent,
                Content = unitPicker
            };

            // Dodawanie do layoutu
            layout.Children.Add(removeButton);
            layout.Children.Add(ingredientEntryFrame);
            layout.Children.Add(amountEntryFrame);
            layout.Children.Add(unitPickerFrame);

            IngredientsList.Children.Add(layout);
        }

        // Funkcja do dodawania kroku
        private void AddStepEntry(string placeholder)
        {
            var layout = new HorizontalStackLayout { Spacing = 15 };

            // Przycisk usuwania
            var removeButton = new Button
            {
                Text = "-",
                FontSize = 30,
                TextColor = Colors.White,
                BackgroundColor = Colors.Red,
                WidthRequest = 40,
                HeightRequest = 40
            };

            // Entry: Krok
            var stepEntryFrame = new Frame
            {
                BorderColor = Colors.White,
                CornerRadius = 5,
                Padding = 5,
                BackgroundColor = Colors.Transparent,
                Content = new Entry
                {
                    Placeholder = placeholder,
                    FontAttributes = FontAttributes.Italic,
                    MaxLength = 500,
                    BackgroundColor = Color.FromHex("#3B3533"),
                    PlaceholderColor = Colors.White,
                    WidthRequest = 670
                }
            };

            layout.Children.Add(removeButton);
            layout.Children.Add(stepEntryFrame);

            StepsList.Children.Add(layout);
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
            var result = await _baseRepository.SaveRecipeAsync(newRecipe);

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
