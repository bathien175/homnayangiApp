using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using UraniumUI.Material.Controls;

namespace homnayangiApp.Views;

public partial class SearchView : ContentPage
{
	public SearchView()
	{
		InitializeComponent();
	}
    private async void PickerField_SelectedValueChangedAsync(object sender, object e)
    {
        var pk = (PickerField)sender;
        var result = (pk.SelectedItem as Province).province_id;
        await dataCity.Instance.getDictricts(result);
        var vm = (ViewModels.SearchViewModel)BindingContext;
        vm.ListDistrict = dataCity.Instance.listDistrict;
        vm.DistrictSelect = vm.ListDistrict[0];
    }
}