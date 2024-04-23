using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ViewModels
{
    public class ListUserCreateViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private ObservableCollection<LocationItem> listLocat = new ObservableCollection<LocationItem>();
        private bool isLoading = false;

        public DelegateCommand backPage { get; }
        public DelegateCommand gotoAddLocation { get; }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public ObservableCollection<LocationItem> ListLocat { get => listLocat; set => SetProperty(ref listLocat, value); }


        public ListUserCreateViewModel()
        {
            _locationService = new LocationService();
            loadLocation();
            backPage = new DelegateCommand(executeBackPageCMD);
            gotoAddLocation = new DelegateCommand(executeAddLocation);
        }

        private async void executeAddLocation()
        {
            IsLoading = true;
            var v = await Task.Run(() => new Views.AdminAddLocationView());
            await Shell.Current.Navigation.PushAsync(v);
            IsLoading = false;
        }

        private async void executeBackPageCMD()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
        public async void loadLocation()
        {
            var u = dataLogin.Instance.currUser;
            List<Models.Location> listget = await Task.Run(() => _locationService.GetCreate(u.Id));
            var listnew2 = new ObservableCollection<Models.LocationItem>();
            await Task.Run(() =>
            {
                if (listget.Count > 0)
                {
                    foreach (var item in listget)
                    {
                        LocationItem model = new LocationItem();
                        model.LocationCurrent = item;
                        listnew2.Add(model);
                    }
                }
                ListLocat = listnew2;
            });
            
        }
    }
}
