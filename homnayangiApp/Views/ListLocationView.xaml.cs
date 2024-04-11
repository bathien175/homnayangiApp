namespace homnayangiApp.Views;


public partial class ListLocationView : ContentPage
{
	public ListLocationView()
	{
		InitializeComponent();
		BindingContext = new ViewModels.ListLocationViewModel();
	}
	public void LoadData(int type)
	{
		var vm = (ViewModels.ListLocationViewModel)BindingContext;
		vm.Mode = type;
	}
}