using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        public bool IsExecuteCMD { get => isExecuteCMD; set => SetProperty(ref isExecuteCMD, value); }

        public MainPageViewModel()
        {
            _locationService = new LocationService();
            loadData();
            GotoListLocationCMD = new DelegateCommand<string>(executeGotoListCMD, canExecuteGoToList);
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
            IsLoading = true;
            int obj = Convert.ToInt32(objs);
            switch (obj)
            {
                case 0:
                    var vm = await Task.Run(() => new ListLocationViewModel() { Mode = 0 });
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ListLocationView() { BindingContext = vm });
                    IsExecuteCMD = false;
                    IsLoading = false;
                    break;
                case 1:
                    var vm1 = await Task.Run(() => new ListLocationViewModel() { Mode = 1 });
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ListLocationView() { BindingContext = vm1 });
                    IsExecuteCMD = false;
                    IsLoading = false;
                    break;
                case 2:
                    var vm2 = await Task.Run(() => new ListLocationViewModel() { Mode = 2 });
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ListLocationView() { BindingContext = vm2 });
                    IsExecuteCMD = false;
                    IsLoading = false;
                    break;
            }
        }

        void loadData()
        {
            NameUser = dataLogin.Instance.currUser.Name;
            ImageByteUser = dataLogin.Instance.currUser.ImageData;
        }
        public async void loadLocation()
        {
            IsLoading = true;
            ListLocationNear.Clear();
            ListLocationByTag.Clear();
            ListLocationUserCreate.Clear();
            List<Models.Location> ListGet = await Task.Run(() => _locationService.GetIsShare());
            await Task.Run(() =>
            {
                if(ListGet.Count > 0)
                {
                    foreach (var location in ListGet)
                    {
                        Models.LocationItem model = new Models.LocationItem();
                        model.LocationCurrent = location;

                        // Kiểm tra xem địa điểm này có nằm trong danh sách lưu trữ của người dùng hay không
                        if (dataLogin.Instance.currUser.SaveStore != null)
                        {
                            if (dataLogin.Instance.currUser.SaveStore.Contains(location.Id))
                            {
                                model.IsSave = true;
                            }
                        }
                        // Thêm đối tượng LocationItem vào danh sách tương ứng
                        if (location.Creator == dataLogin.Instance.currUser.Id)
                        {
                            if (ListLocationUserCreate.Count < 5)
                            {
                                ListLocationUserCreate.Add(model);
                            }
                        }
                        else
                        {
                            if (location.Province == dataLogin.Instance.currUser.City)
                            {
                                if (ListLocationNear.Count < 5)
                                {
                                    ListLocationNear.Add(model);
                                }
                            }

                            if (location.Tags.Any(value => dataLogin.Instance.currUser.Tags.Contains(value)))
                            {
                                if (ListLocationByTag.Count < 5)
                                {
                                    ListLocationByTag.Add(model);
                                }
                            }
                        }
                    }
                }
            });
            IsLoading = false;
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
                var imgt = Convert.FromBase64String(ImageByteUser);
                MemoryStream stream = new(imgt);
                ImageSource image = ImageSource.FromStream(() => stream);
                ImageUserSource = image;
            }
        }
    }
}
