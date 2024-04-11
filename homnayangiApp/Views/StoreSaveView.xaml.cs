namespace homnayangiApp.Views;

public partial class StoreSaveView : ContentPage
{
	public StoreSaveView()
	{
		InitializeComponent();
		BindingContext = new ViewModels.SaveLocationViewModel();
	}

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
		var vm = (ViewModels.SaveLocationViewModel)BindingContext;
		vm.LoadData();
    }
}