using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using UraniumUI.Material.Controls;

namespace homnayangiApp.Views;

public partial class AdminAddLocationView : ContentPage
{
	public AdminAddLocationView()
	{
		InitializeComponent();
	}

    private async void PickerField_SelectedValueChanged(object sender, object e)
    {
        var pk = (PickerField)sender;
        var result = (pk.SelectedItem as Province).province_id;
        await dataCity.Instance.getDictricts(result);
        var vm = (ViewModels.AddLocationViewModel)BindingContext;
        vm.ListDistrict = dataCity.Instance.listDistrict;
        vm.DistrictSelect = vm.ListDistrict[0];
    }
}