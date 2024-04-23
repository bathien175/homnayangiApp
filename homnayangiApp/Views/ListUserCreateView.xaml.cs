namespace homnayangiApp.Views;

public partial class ListUserCreateView : ContentPage
{
	public ListUserCreateView()
	{
		InitializeComponent();
	}

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
		var vm = (ViewModels.ListUserCreateViewModel)BindingContext;
		vm.loadLocation();
    }
}