using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using System.Collections.ObjectModel;

namespace homnayangiApp.ViewModels
{
    public class MainPageViewModel: BaseViewModel
    {
        private bool isLoading = false;
        private readonly ILocationService _locationService;
        private string nameUser = string.Empty;
        private string? imageByteUser;
        private ImageSource imageUserSource;
        private ObservableCollection<Models.LocationItem> listLocationNear = [];
        private ObservableCollection<Models.LocationItem> listLocationByTag = [];
        private ObservableCollection<Models.LocationItem> listLocationUserCreate = [];
        private bool isExecuteCMD = false;
        private string textSearch = string.Empty;

        public string NameUser { get => nameUser; set => SetProperty(ref nameUser, value); }
        public string? ImageByteUser { get => imageByteUser; set
            {
                SetProperty(ref imageByteUser, value);
                loadImage();
            }
        }
        public ImageSource ImageUserSource { get => imageUserSource; set => SetProperty(ref imageUserSource, value); }
        public ObservableCollection<Models.LocationItem> ListLocationNear { get => listLocationNear; set => SetProperty(ref listLocationNear, value); }
        public ObservableCollection<Models.LocationItem> ListLocationByTag { get => listLocationByTag; set => SetProperty(ref listLocationByTag, value); }
        public ObservableCollection<Models.LocationItem> ListLocationUserCreate { get => listLocationUserCreate; set => SetProperty(ref listLocationUserCreate, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }


        public DelegateCommand<string> GotoListLocationCMD { get; }
        public DelegateCommand SearchLocationCMD { get; }
        public bool IsExecuteCMD { get => isExecuteCMD; set => SetProperty(ref isExecuteCMD, value); }
        public string TextSearch { get => textSearch; set => SetProperty(ref textSearch, value); }

        public MainPageViewModel()
        {
            _locationService = new LocationService();
            loadData();
            GotoListLocationCMD = new DelegateCommand<string>(executeGotoListCMD, canExecuteGoToList);
            SearchLocationCMD = new DelegateCommand(executeSearchCMD);
        }

        private async void executeSearchCMD()
        {
            if(TextSearch != string.Empty)
                await Shell.Current.GoToAsync($"//HomeApp/SearchView?TextSearchByMainPage={TextSearch}");
        }

        private bool canExecuteGoToList(string args)
        {
            int arg = Convert.ToInt32(args);
            switch (arg)
            {
                case 0:
                    if (ListLocationNear.Count < 5)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 1:
                    if (ListLocationByTag.Count < 5)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 2:
                    if (ListLocationUserCreate.Count < 5)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default: return false;
            }
        }

        private async void executeGotoListCMD(string objs)
        {
            if (IsExecuteCMD)
                return;

            IsExecuteCMD = true;
            int obj = Convert.ToInt32(objs);
            switch (obj)
            {
                case 0:
                    IsLoading = true;
                    var v = await Task.Run(() => new ListLocationView()); 
                    await Shell.Current.Navigation.PushModalAsync(v);
                    v.LoadData(0);
                    IsLoading = false;
                    IsExecuteCMD = false;
                    break;
                case 1:
                    IsLoading = true;
                    var v1 = await Task.Run(() => new ListLocationView());
                    await Shell.Current.Navigation.PushModalAsync(v1);
                    v1.LoadData(1);
                    IsLoading = false;
                    IsExecuteCMD = false;
                    break;
                case 2:
                    IsLoading = true;
                    var v2 = await Task.Run(() => new ListLocationView());
                    await Shell.Current.Navigation.PushModalAsync(v2);
                    v2.LoadData(2);
                    IsLoading = false;
                    IsExecuteCMD = false;
                    break;
            }
        }

        public void loadData()
        {
            NameUser = dataLogin.Instance.currUser.Name;
            ImageByteUser = dataLogin.Instance.currUser.ImageData;
            loadLocation();
        }

        async void loadLocation()
        {
            await loadnear();
            await loadbytag();
            await loadbycreate();
        }
        async Task loadnear()
        {
            ListLocationNear.Clear();
            List<Models.Location> locationnear = await _locationService.GetNear(dataLogin.Instance.currUser.City, dataLogin.Instance.currUser.Dictrict);
            if (locationnear.Count != 0)
            {
                foreach (var location in locationnear.Take(5))
                {
                    Models.LocationItem model = new Models.LocationItem
                    {
                        LocationCurrent = location
                    };
                    // Kiểm tra xem địa điểm này có nằm trong danh sách lưu trữ của người dùng hay không
                    if (dataLogin.Instance.currUser.SaveStore != null)
                    {
                        if (dataLogin.Instance.currUser.SaveStore.Contains(location.Id))
                        {
                            model.IsSave = true;
                        }
                    }
                    ListLocationNear.Add(model);
                }
            }
        }
        async Task loadbytag()
        {
            ListLocationByTag.Clear();
            List<Models.Location> locationtag = await _locationService.GetByTag(dataLogin.Instance.currUser.Tags);
            if (locationtag.Count != 0)
            {
                foreach (var location in locationtag.Take(5))
                {
                    Models.LocationItem model = new Models.LocationItem
                    {
                        LocationCurrent = location
                    };
                    // Kiểm tra xem địa điểm này có nằm trong danh sách lưu trữ của người dùng hay không
                    if (dataLogin.Instance.currUser.SaveStore != null)
                    {
                        if (dataLogin.Instance.currUser.SaveStore.Contains(location.Id))
                        {
                            model.IsSave = true;
                        }
                    }
                    ListLocationByTag.Add(model);
                }
            }
        }
        async Task loadbycreate()
        {
            ListLocationUserCreate.Clear();
            List<Models.Location> locationcreate = await _locationService.GetCreate(dataLogin.Instance.currUser.Id);
            if (locationcreate.Count != 0)
            {
                foreach (var location in locationcreate.Take(5))
                {
                    Models.LocationItem model = new Models.LocationItem
                    {
                        LocationCurrent = location
                    };
                    // Kiểm tra xem địa điểm này có nằm trong danh sách lưu trữ của người dùng hay không
                    if (dataLogin.Instance.currUser.SaveStore != null)
                    {
                        if (dataLogin.Instance.currUser.SaveStore.Contains(location.Id))
                        {
                            model.IsSave = true;
                        }
                    }
                    ListLocationUserCreate.Add(model);
                }
            }
        }
        public void loadImage()
        {
            if (ImageByteUser == null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "noimage.png");
                ImageUserSource = ImageSource.FromFile(imagePath);
            }
            else
            {
                ImageUserSource = ImageByteUser;
            }
        }
    }
}
