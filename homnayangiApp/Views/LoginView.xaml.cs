using homnayangiApp.ModelService;

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

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        txtPass.Text = string.Empty;
        txtPhone.Text = string.Empty;
        if(txtPhone.IsFocused == true)
            txtPhone.Unfocus();
    }
}