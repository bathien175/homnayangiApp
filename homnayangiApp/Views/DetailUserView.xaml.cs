namespace homnayangiApp.Views;

public partial class DetailUserView : ContentPage
{
	public DetailUserView()
	{
		InitializeComponent();
	}

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
		var vm = (ViewModels.DetailUserViewModel)BindingContext;
		vm.LoadLocation();
    }
}