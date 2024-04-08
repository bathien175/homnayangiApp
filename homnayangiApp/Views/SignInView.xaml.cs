namespace homnayangiApp.Views;

public partial class SignInView : ContentPage
{
    public SignInView()
	{
		InitializeComponent();
        BindingContext = new ViewModels.SignInViewModel();
    }
}