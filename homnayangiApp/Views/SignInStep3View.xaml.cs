using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using System.Net.WebSockets;
using UraniumUI.Material.Controls;

namespace homnayangiApp.Views;

public partial class SignInStep3View : ContentPage
{
	public SignInStep3View()
	{
		InitializeComponent();
		BindingContext = new ViewModels.SignInStep3ViewModel();
    }

    private async void PickerField_SelectedValueChanged(object sender, object e)
    {
        var pk = (PickerField)sender;
        var result = (pk.SelectedItem as Province).province_id;
        await dataCity.Instance.getDictricts(result);
        var vm = (ViewModels.SignInStep3ViewModel)BindingContext;
        vm.ListDistrict = dataCity.Instance.listDistrict;
        vm.DistrictSelect = vm.ListDistrict[0];
    }
}