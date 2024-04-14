
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace homnayangiApp.Views;

public partial class MapLocation : ContentPage
{
	private string CurrentAddress = string.Empty;
	private string NameAddress = string.Empty;
	public MapLocation(string address, string name)
	{
		InitializeComponent();
        CurrentAddress = address;
        NameAddress = name;
        ShowLocationOnMap();
    }

    private async void ShowLocationOnMap()
    {
        IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(CurrentAddress);
        Location location = locations?.FirstOrDefault();
        myMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100)));

        var pin = new Pin
        {
            Address = CurrentAddress,
            Type = PinType.Place,
            Label = NameAddress,
            Location = location
        };

        myMap.Pins.Add(pin);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopModalAsync(true);
    }
}