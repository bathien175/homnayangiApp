using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using System.Collections.ObjectModel;

namespace homnayangiApp.ViewModels
{
    public class DetailUserViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private User userCurrent = new User();
        private DelegateCommand? _backPage;
        private ObservableCollection<LocationItem> listLocation = new ObservableCollection<LocationItem>();
        private ObservableCollection<LocationItem> listLocationFillter = new ObservableCollection<LocationItem>();
        private string textSearch = string.Empty;
        private bool isLoading = false;
        private bool isExecuteCMD = false;

        public User UserCurrent { get => userCurrent; set => SetProperty(ref userCurrent, value); }
        public DelegateCommand? backPage { get => _backPage; set => SetProperty(ref _backPage, value); }
        public ObservableCollection<LocationItem> ListLocation { get => listLocation; 
            set 
            { 
                SetProperty(ref listLocation, value);
                loadFilter();
            } 
        }

        private void loadFilter()
        {
            ListLocationFillter = new ObservableCollection<LocationItem>(ListLocation.Where(x => x.LocationCurrent.Name.ToLower().Contains(TextSearch.ToLower())).ToList());
        }

        public ObservableCollection<LocationItem> ListLocationFillter { get => listLocationFillter; set => SetProperty(ref listLocationFillter, value); }
        public string TextSearch { get => textSearch; set { SetProperty(ref textSearch, value); loadFilter(); } }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public bool IsExecuteCMD { get => isExecuteCMD; set => SetProperty(ref isExecuteCMD, value); }

        public DetailUserViewModel(User u)
        {
            _locationService = new LocationService();
            UserCurrent = u;
            backPage = new DelegateCommand(executebackPageCMD);
        }

        private async void executebackPageCMD()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public async void LoadLocation()
        {
            IsLoading = true;
            var list = await Task.Run(() => _locationService.GetCreateShare(UserCurrent.Id));
            var listnew = new ObservableCollection<LocationItem>();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    LocationItem model = new LocationItem();
                    model.LocationCurrent = item;
                    if (dataLogin.Instance.currUser.SaveStore != null)
                    {
                        if (dataLogin.Instance.currUser.SaveStore.Where(x => x == item.Id).FirstOrDefault() != null)
                        {
                            model.IsSave = true;
                        }
                    }
                    listnew.Add(model);
                }
            }
            ListLocation = listnew;
            IsLoading = false;
        }
    }
}
