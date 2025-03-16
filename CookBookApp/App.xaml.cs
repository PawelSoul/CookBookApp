using CookBookApp.Data;
using CookBookApp.Repositories.Interfaces;

namespace CookBookApp
{
    public partial class App : Application
    {

        private readonly IBaseRepository _iBaseRepository;

        public App(IBaseRepository recipeRepository)
        {
            InitializeComponent();
            _iBaseRepository = recipeRepository;

            MainPage = new NavigationPage(new MainPage(_iBaseRepository)); // Włączamy nawigację
        }
    }
}
