using homnayangiApp.CustomControls;
using homnayangiApp.ViewModels;

namespace homnayangiApp.Views;

public partial class PersonalView : ContentPage
{
	public PersonalView()
	{
		InitializeComponent();
		BindingContext = new AccountManagerViewModel();
	}

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        BindingContext = new AccountManagerViewModel();
    }
}