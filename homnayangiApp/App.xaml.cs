using homnayangiApp.CustomControls;

namespace homnayangiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            dataCity.Instance.ReloadData();
            MainPage = new AppShell();
        }
    }
}
