using System.Collections.ObjectModel;
using CookBookApp.Models;
using CookBookApp.Repositories.Interfaces;

namespace CookBookApp
{
    public partial class Add_Recipe : ContentPage
    {
        public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>();

        //public Dictionary<string, double> Ingredients { get; set; } = new Dictionary<string, double>();
        //public Dictionary<string, double?> InstructionSteps { get; set; } = new Dictionary<string, double?>();

        IBaseRepository _baseRepository;

        Recipe _newRecipe = new Recipe();
        int _stepCount = 1;

        public Add_Recipe(IBaseRepository baseRepository)// Konstruktor, który dostaje DbContext z DI
        {
            InitializeComponent();
            ImagesCollectionView.ItemsSource = Images;
            _baseRepository = baseRepository;

            _newRecipe = new Recipe();
            _newRecipe.Ingredients = new List<Ingredient>();
            _newRecipe.InstructionSteps = new List<InstructionStep>();
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
            if(!ValidationAddIngredient())
            {
                return;
            }
            var newIngredient = new Ingredient { Name = IngredientEntry.Text, Quantity = double.Parse(IngredientAmount.Text), Unit = IngredientUnitPicker.SelectedItem.ToString() };

            _newRecipe.Ingredients.Add(newIngredient);

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

            var unitPicker = new Entry
            {
                Placeholder = unit,
                Keyboard = Keyboard.Numeric,
                BackgroundColor = Color.FromHex("#3B3533"),
                PlaceholderColor = Colors.White,
                WidthRequest = 110
            };

            removeButton.Clicked += (s, e) => IngredientsList.Children.Remove(layout);

            layout.Children.Add(removeButton);
            layout.Children.Add(ingredientEntry);
            layout.Children.Add(amountEntry);
            layout.Children.Add(unitPicker);

            IngredientsList.Children.Add(layout);
        }

        private bool ValidationAddIngredient()
        {
            // Walidacja pól
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(IngredientEntry.Text))
            {
                IngredientEntry.BackgroundColor = Colors.Red; // Czerwona ramka
                isValid = false;
            }
            else
            {
                IngredientEntry.BackgroundColor = Colors.Transparent;
            }

            if (string.IsNullOrWhiteSpace(IngredientAmount.Text) || !double.TryParse(IngredientAmount.Text, out double quantity))
            {
                IngredientAmount.BackgroundColor = Colors.Red;
                isValid = false;
            }
            else
            {
                IngredientAmount.BackgroundColor = Colors.Transparent;
            }

            if (IngredientUnitPicker.SelectedItem == null)
            {
                IngredientUnitPicker.BackgroundColor = Colors.Red;
                isValid = false;
            }
            else
            {
                IngredientUnitPicker.BackgroundColor = Colors.Transparent;
            }
            return isValid;
        }

        // Funkcja do dodawania nowego kroku z czasem wykonania
        private void AddStepEntry(string placeholder, string unit)
        {
            
            if (!ValidAddStep())
            {
                return;
            }
            _stepCount += 1;
            var newIngredientStep = new InstructionStep { StepNumber = int.Parse(StepNumberLabel.Text), Description = StepEntry.Text };

            _newRecipe.InstructionSteps.Add(newIngredientStep);
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

            var numberStep = new Entry
            {
                Text = _stepCount.ToString(),
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                Keyboard = Keyboard.Numeric,
                BackgroundColor = Color.FromHex("#3B3533"),
                WidthRequest = 40
            };

            removeButton.Clicked += (s, e) => StepsList.Children.Remove(layout);

            layout.Children.Add(removeButton);
            layout.Children.Add(numberStep);
            layout.Children.Add(stepEntry);


            StepsList.Children.Add(layout);
        }

        private bool ValidAddStep()
        {
            bool isValid = true;

            // Walidacja numeru kroku
            if (string.IsNullOrWhiteSpace(StepNumberLabel.Text) || !int.TryParse(StepNumberLabel.Text, out int stepNumber))
            {
                StepNumberLabel.BackgroundColor = Colors.Red;
                isValid = false;
            }
            else
            {
                StepNumberLabel.BackgroundColor = Colors.Transparent;
            }

            // Walidacja opisu kroku
            if (string.IsNullOrWhiteSpace(StepEntry.Text))
            {
                StepEntry.BackgroundColor = Colors.Red;
                isValid = false;
            }
            else
            {
                StepEntry.BackgroundColor = Colors.Transparent;
            }
            return isValid;
        }

        // Usuwanie pierwszego sk³adnika
        private void OnRemoveIngredientClicked(object sender, EventArgs e)
        {
            FirstIngredientRow.IsVisible = false;
        }

        // Usuwanie pierwszego kroku
        private void OnRemoveStepClicked(object sender, EventArgs e)
        {
            StepRow.IsVisible = false;
        }

        //Zapis Przepisu
        private async void OnSaveRecipeClicked(object sender, EventArgs e)
        {

            var result = await _baseRepository.AddRecipeAsync(_newRecipe);

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
