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
        private ObservableCollection<LocationItem> listLocatFilter = new ObservableCollection<LocationItem>();
        private string textSearch = string.Empty;
        private bool isLoading = false;
        private bool isExecuteCMD = false;
        public DelegateCommand backPage { get; }
        public DelegateCommand gotoAddLocation { get; }
        public DelegateCommand<string> UpdateLocation { get; }
        public DelegateCommand<string> DeleteLocation { get; }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public ObservableCollection<LocationItem> ListLocat { get => listLocat; set { SetProperty(ref listLocat, value); filterListLocate(); } }
        public bool IsExecuteCMD { get => isExecuteCMD; set => SetProperty(ref isExecuteCMD, value); }
        public ObservableCollection<LocationItem> ListLocatFilter { get => listLocatFilter; set => SetProperty(ref listLocatFilter, value); }
        public string TextSearch { get => textSearch; 
            set 
            { 
                SetProperty(ref textSearch, value);
                filterListLocate();
            } 
        }

        private void filterListLocate()
        {
            ListLocatFilter = new ObservableCollection<LocationItem>(ListLocat.Where(x => x.LocationCurrent.Name.ToLower().Contains(TextSearch.ToLower())));
        }

        public ListUserCreateViewModel()
        {
            _locationService = new LocationService();
            loadLocation();
            UpdateLocation = new DelegateCommand<string>(executeUpdateCMD);
            DeleteLocation = new DelegateCommand<string>(executeDeleteCMD);
            backPage = new DelegateCommand(executeBackPageCMD);
            gotoAddLocation = new DelegateCommand(executeAddLocation);
        }


        private async void executeDeleteCMD(string s)
        {
            var result = await Shell.Current.DisplayAlert("Chờ đã!","Bạn có thực sự muốn xóa đi địa điểm này chứ?","Có","Không");
            if(result)
            {
                await _locationService.Remove(s);
                loadLocation();
            }
        }

        private async void executeUpdateCMD(string obj)
        {
            if (IsExecuteCMD == true)
                return;

            IsExecuteCMD = true;
            var vm = await Task.Run(() => new ViewModels.AddLocationViewModel());
            vm.LocateIdCurrent = obj;
            var v = await Task.Run(() => new Views.AdminAddLocationView() { BindingContext = vm});
            await Shell.Current.Navigation.PushAsync(v);
            IsExecuteCMD = false;
        }

        private async void executeAddLocation()
        {
            if (IsExecuteCMD == true)
                return;

            IsExecuteCMD = true;
            var vm = await Task.Run(() => new ViewModels.AddLocationViewModel());
            vm.LocateIdCurrent = string.Empty;
            var v = await Task.Run(() => new Views.AdminAddLocationView() { BindingContext = vm });
            await Shell.Current.Navigation.PushAsync(v);
            IsExecuteCMD = false;
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
                        model.IsActive = item.IsShare;
                        listnew2.Add(model);
                    }
                }
                ListLocat = listnew2;
            });
            
        }
    }
}
