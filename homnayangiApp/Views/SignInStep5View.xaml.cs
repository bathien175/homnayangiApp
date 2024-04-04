namespace homnayangiApp.Views;

public partial class SignInStep5View : ContentPage
{
	public SignInStep5View()
	{
		InitializeComponent();
	}
    protected override bool OnBackButtonPressed()
    {
        // Ngăn chặn sự kiện "Trở lại" từ xảy ra
        return true;
    }
}