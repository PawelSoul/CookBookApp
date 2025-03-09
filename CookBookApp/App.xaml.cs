using CookBookApp.Data;

namespace CookBookApp
{
    public partial class App : Application
    {
        private readonly AppDbContext _dbContext; // Pole na DbContext
        public App(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            MainPage = new NavigationPage(new MainPage(_dbContext)); // Włączamy nawigację
        }
    }
}
