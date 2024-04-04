using homnayangiApp.CustomControls;
using homnayangiApp.Views;

namespace homnayangiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            dataCity.Instance.ReloadData();
            MainPage = new NavigationPage(new LoginView());
        }
    }
}
