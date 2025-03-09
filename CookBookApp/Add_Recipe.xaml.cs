using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace CookBookApp
{
    public partial class AddRecipePage : ContentPage
    {
        public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>();

        public AddRecipePage()
        {
            InitializeComponent();
            ImagesCollectionView.ItemsSource = Images;
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
            string ingredients = IngredientsEditor.Text;
            string steps = StepsEditor.Text;

            if (string.IsNullOrWhiteSpace(recipeName) || string.IsNullOrWhiteSpace(ingredients) || string.IsNullOrWhiteSpace(steps))
            {
                await DisplayAlert("B��d", "Prosz� uzupe�ni� wszystkie pola!", "OK");
                return;
            }

            // Tutaj mo�na doda� logik� zapisu do bazy danych lub lokalnego pliku
            await DisplayAlert("Sukces", "Przepis zosta� zapisany!", "OK");
            await Navigation.PopAsync();
        }
    }
}
