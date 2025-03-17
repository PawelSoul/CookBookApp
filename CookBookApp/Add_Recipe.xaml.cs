using System.Collections.ObjectModel;
using CookBookApp.Models;
using CookBookApp.Repositories.Interfaces;

namespace CookBookApp
{
    public partial class Add_Recipe : ContentPage
    {
        IBaseRepository _baseRepository;

        Recipe _newRecipe = new Recipe();
        int _stepCount = 1;

        public Add_Recipe(IBaseRepository baseRepository)// Konstruktor, który dostaje DbContext z DI
        {
            InitializeComponent();
            _baseRepository = baseRepository;

            _newRecipe = new Recipe();
            _newRecipe.Ingredients = new List<Ingredient>();
            _newRecipe.InstructionSteps = new List<InstructionStep>();
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
                FontSize = 20,
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
                    FontAttributes = FontAttributes.Italic,
                    MaxLength = 200,
                    BackgroundColor = Color.FromHex("#3B3533"),
                    PlaceholderColor = Colors.White,
                    WidthRequest = 400
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
                    FontAttributes = FontAttributes.Italic
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

        private bool ValidationAddIngredient()
        {
            bool isValid = true;

            foreach (var ingredient in IngredientsList.Children)
            {
                if (ingredient is HorizontalStackLayout row)
                {
                    // Pobierz Entry z nazw¹ sk³adnika
                    var ingredientFrame = row.Children[1] as Frame;
                    var ingredientEntry = ingredientFrame?.Content as Entry;
                    var ingredientName = ingredientEntry?.Text;

                    // Pobierz Entry z iloœci¹
                    var amountFrame = row.Children[2] as Frame;
                    var amountEntry = amountFrame?.Content as Entry;
                    var amountValue = amountEntry?.Text;

                    // Pobierz Picker z jednostk¹
                    var pickerFrame = row.Children[3] as Frame;
                    var unitPicker = pickerFrame?.Content as Picker;
                    var selectedUnit = unitPicker?.SelectedItem?.ToString();

                    // Walidacja nazwy sk³adnika
                    if (string.IsNullOrWhiteSpace(ingredientName))
                    {
                        ingredientFrame.BorderColor = Colors.Red;
                        isValid = false;
                    }
                    else
                    {
                        ingredientFrame.BorderColor = Colors.Transparent;
                    }

                    // Walidacja iloœci
                    if (string.IsNullOrWhiteSpace(amountValue) || !double.TryParse(amountValue, out double quantity) || quantity <= 0.00)
                    {
                        amountFrame.BorderColor = Colors.Red;
                        isValid = false;
                    }
                    else
                    {
                        amountFrame.BorderColor = Colors.Transparent;
                    }

                    // Walidacja jednostki
                    if (string.IsNullOrWhiteSpace(selectedUnit))
                    {
                        pickerFrame.BorderColor = Colors.Red;
                        isValid = false;
                    }
                    else
                    {
                        pickerFrame.BorderColor = Colors.Transparent;
                    }
                }
            }

            return isValid;
        }

        // Funkcja do dodawania nowego kroku z czasem wykonania
        private void AddStepEntry(string placeholder, string unit)
        {
            _stepCount += 1;
            var layout = new HorizontalStackLayout { Spacing = 5 };

            // Przycisk usuwania
            var removeButton = new Button
            {
                Text = "-",
                FontSize = 20,
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
            removeButton.Clicked += (s, e) =>
            {
                StepsList.Children.Remove(layout);
                _stepCount -= 1;
            };

            // Dodanie do layoutu
            layout.Children.Add(removeButton);
            layout.Children.Add(stepEntryFrame);

            StepsList.Children.Add(layout);
        }

        private bool ValidAddStep(string step)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(step))
            {
                StepEntryFrame.BorderColor = Colors.Red;
                isValid = false;
            }
            else
            {
                StepEntryFrame.BorderColor = Colors.Transparent;
            }
            return isValid;
        }

        //Zapis Przepisu
        private async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            if (!ValidationAddIngredient()) return;
            

            foreach (var step in StepsList.Children)
            {
                if (step is HorizontalStackLayout row)
                {
                    // Pobierz Entry z nazw¹ sk³adnika
                    var stepFrame = row.Children[1] as Frame;
                    var stepEntry = stepFrame?.Content as Entry;
                    var stepName = stepEntry?.Text;

                    if (!ValidAddStep(stepName)) continue;
                }
            }

            //// Dodanie nowego sk³adnika do modelu
            //var newIngredient = new Ingredient
            //{
            //    Name = IngredientEntry.Text,
            //    Quantity = double.Parse(IngredientAmount.Text),
            //    Unit = IngredientUnitPicker.SelectedItem.ToString()
            //};

            //_newRecipe.Ingredients.Add(newIngredient);

            //// Dodanie nowego sk³adnika do modelu
            //var newInstructionStep = new InstructionStep
            //{
            //    StepNumber = _stepCount,
            //    Description = StepEntry.Text
            //};

            //_newRecipe.InstructionSteps.Add(newInstructionStep);


            //var result = await _baseRepository.AddRecipeAsync(_newRecipe);

            //if (result)
            //{
            //    // Sukces
            //}
            //else
            //{
            //    // Obs³uga b³êdu
            //}
        }


    }
}
