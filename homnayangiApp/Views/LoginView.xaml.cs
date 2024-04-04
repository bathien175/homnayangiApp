namespace homnayangiApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();
	}
    protected override bool OnBackButtonPressed()
    {
        // Ngăn chặn sự kiện "Trở lại" từ xảy ra
        return true;
    }
}