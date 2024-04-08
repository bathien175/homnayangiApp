using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.Views;
using System;
using UraniumUI.Material.Controls;

namespace homnayangiApp
{
    public partial class App : Application
    {
        public static Realms.Sync.App RealmApp;
        public App()
        {
            InitializeComponent();
            dataCity.Instance.ReloadData();
            MainPage = new AppShell();
        }
    }
}
