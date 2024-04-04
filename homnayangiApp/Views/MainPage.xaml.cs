
namespace homnayangiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var vm = (ViewModels.MainPageViewModel)BindingContext;
            vm.loadImage();
            vm.loadLocation();
        }
    }

}
