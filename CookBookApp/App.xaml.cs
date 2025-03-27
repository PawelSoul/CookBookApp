using CookBookApp.Data;
using CookBookApp.Repositories.Interfaces;

namespace CookBookApp
{
    public partial class App : Application
    {

        private readonly IRecipeRepository _iBaseRepository;

        public App(IRecipeRepository recipeRepository)
        {
            InitializeComponent();
            _iBaseRepository = recipeRepository;

            MainPage = new NavigationPage(new MainPage(_iBaseRepository)); // Włączamy nawigację
        }
    }
}
