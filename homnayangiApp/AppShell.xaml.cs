using homnayangiApp.Views;

namespace homnayangiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("listLocation", typeof(ListLocationView));
            Routing.RegisterRoute(nameof(AdminAddLocationView), typeof(AdminAddLocationView));
            Routing.RegisterRoute(nameof(SettingView), typeof(SettingView));
        }
    }
}
