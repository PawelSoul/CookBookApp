using System.Collections.ObjectModel;
using System.Net.WebSockets;
using CookBookApp.Models;
using CookBookApp.Repositories.Interfaces;

namespace CookBookApp
{
    public partial class Add_Recipe : ContentPage
    {
        IBaseRepository _baseRepository;

        Recipe _newRecipe = new Recipe();

        public Add_Recipe(IBaseRepository baseRepository)// Konstruktor, który dostaje DbContext z DI
        {
            InitializeComponent();
            _baseRepository = baseRepository;

            //_newRecipe = new Recipe();
            //_newRecipe.Ingredients = new List<Ingredient>();
            //_newRecipe.InstructionSteps = new List<InstructionStep>();
        }

        private void OnAddIngredientClicked(object sender, EventArgs e)
        {
            AddIngredientEntry("Dodaj sk³adnik ...", "0,00");
        }

        private void OnAddStepsClicked(object sender, EventArgs e)
        {
            AddStepEntry("Dodaj krok ...", "0");
        }

        // Funkcja do dodawania nowego sk³adnika z gramatur¹
        private void AddIngredientEntry(string placeholderNazwa, string placeholderAmount)
        {
            // Tworzymy layout dla nowego sk³adnika
            var layout = new HorizontalStackLayout { Spacing = 5 };

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

            removeButton.Clicked += (s, e) => IngredientsList.Children.Remove(layout);

            // Entry: Nazwa sk³adnika
            var ingredientEntryFrame = new Frame
            {
                BorderColor = Colors.White,
                CornerRadius = 10,
                Padding = 5,
                BackgroundColor = Colors.Transparent,
                Content = new Entry
                {
                    Text = "",
                    Placeholder = placeholderNazwa,
                    MaxLength = 200,
                    BackgroundColor = Color.FromHex("#3B3533"),
                    PlaceholderColor = Colors.White,
                    WidthRequest = 475
                }
            };

            // Entry: Iloœæ
            var amountEntryFrame = new Frame
            {
                BorderColor = Colors.White,
                CornerRadius = 10,
                Padding = 5,
                BackgroundColor = Colors.Transparent,
                Content = new Entry
                {
                    Text = "",
                    Placeholder = placeholderAmount,
                    Keyboard = Keyboard.Numeric,
                    BackgroundColor = Color.FromHex("#3B3533"),
                    PlaceholderColor = Colors.White,
                    WidthRequest = 80,
                }
            };

            // Picker: Jednostka
            var unitPicker = new Picker
            {
                WidthRequest = 110,
                BackgroundColor = Color.FromHex("#3B3533"),
                TextColor = Colors.White,
                ItemsSource = new List<string> { "g", "ml", "szt", "³y¿ka", "szklanka" },
                SelectedIndex = -1
            };

            var unitPickerFrame = new Frame
            {
                BorderColor = Colors.White,
                CornerRadius = 10,
                Padding = 5,
                BackgroundColor = Colors.Transparent,
                Content = unitPicker
            };

            layout.Children.Add(removeButton);
            layout.Children.Add(ingredientEntryFrame);
            layout.Children.Add(amountEntryFrame);
            layout.Children.Add(unitPickerFrame);

            IngredientsList.Children.Add(layout);
        }

        

        // Funkcja do dodawania nowego kroku z czasem wykonania
        private void AddStepEntry(string placeholder, string unit)
        {
            var layout = new HorizontalStackLayout { Spacing = 5 };

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

            // Opis kroku (Entry w ramce)
            var stepEntryFrame = new Frame
            {
                BorderColor = Colors.White,
                CornerRadius = 10,
                Padding = 5,
                BackgroundColor = Colors.Transparent,
                Content = new Entry
                {
                    Text = "",
                    Placeholder = placeholder,
                    FontAttributes = FontAttributes.Italic,
                    MaxLength = 200,
                    BackgroundColor = Color.FromHex("#3B3533"),
                    PlaceholderColor = Colors.White,
                    WidthRequest = 700
                }
            };

            // Obs³uga usuwania kroku
            removeButton.Clicked += (s, e) => StepsList.Children.Remove(layout);

            // Dodanie do layoutu
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

        private bool ValidAddStep()
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

        //Zapis Przepisu
        private async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            if (!ValidateAll()) return;

            int _stepCount = 0;
            var newRecipe = new Recipe { RecipeName = RecipeNameEntry.Text };

            // Dodanie nowego sk³adnika do modelu
            foreach (var ingredient in IngredientsList.Children)
            {
                if (ingredient is HorizontalStackLayout row)
                {
                    Frame? ingredientFrame, amountFrame, pickerFrame;
                    string? ingredientName, amountValue, selectedUnit;
                    GetIngredientDateFromEntry(row, out ingredientFrame, out ingredientName, out amountFrame, out amountValue, out pickerFrame, out selectedUnit);

                    var newIngredient = new Ingredient
                    {
                        Name = ingredientName,
                        Quantity = double.Parse(amountValue),
                        Unit = selectedUnit
                    };

                    _newRecipe.Ingredients.Add(newIngredient);
                }
            }

            // Dodanie nowego kroku do modelu
            foreach (var step in StepsList.Children)
            {
                _stepCount += 1;
                if (step is HorizontalStackLayout row)
                {
                    var stepFrame = row.Children[1] as Frame;
                    var stepEntry = stepFrame?.Content as Entry;
                    var stepDescription = stepEntry?.Text;

                    if (!string.IsNullOrWhiteSpace(stepDescription))
                    {
                        var newInstructionStep = new InstructionStep
                        {
                            StepNumber = _stepCount,
                            Description = StepEntry.Text
                        };

                        _newRecipe.InstructionSteps.Add(newInstructionStep);
                    }
                }
            }

            // Zapis do bazy
            var result = await _baseRepository.SaveRecipeAsync(newRecipe);

            if (result)
            {
                await DisplayAlert("Sukces", "Przepis zapisany!", "OK");
            }
            else
            {
                await DisplayAlert("B³¹d", "Wyst¹pi³ problem podczas zapisu.", "OK");
            }
        }

        private bool ValidateAll()
        {
            bool isStepsValid = ValidAddStep();
            bool isIngredientsValid = ValidationAddAllIngredient();

            return isStepsValid && isIngredientsValid;
        }
    }
}
