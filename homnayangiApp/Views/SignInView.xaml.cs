using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels;

namespace homnayangiApp.Views;

public partial class SignInView : ContentPage
{
    public SignInView()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        var vm = (ViewModels.AccountViewModel)BindingContext;
        vm.BackToLoginCmd.Execute();
    }
}