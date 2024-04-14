
using homnayangiApp.ModelService;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Net;

namespace homnayangiApp.Views;

public partial class MapCurrent : ContentPage
{
	public MapCurrent()
	{
		InitializeComponent();
        ShowUserLocationOnMap();

    }

    private async void ShowUserLocationOnMap()
    {
        //IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync("21/22 Trịnh Thị Miếng, ấp Thới Tứ, xã Thới Tam Thôn, huyện Hóc Môn, Thành Phố Hồ Chí Minh");
        //Location location = locations?.FirstOrDefault();
        var geolocationRequest = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
        var location = await Geolocation.GetLocationAsync(geolocationRequest);
        myMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100)));
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomeApp/MainPage");
    }
}