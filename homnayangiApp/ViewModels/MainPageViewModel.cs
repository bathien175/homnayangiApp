using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
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
        private ObservableCollection<Models.LocationItem> listLocationNear = new ObservableCollection<Models.LocationItem>();
        private ObservableCollection<Models.LocationItem> listLocationByTag = new ObservableCollection<Models.LocationItem>();
        private ObservableCollection<Models.LocationItem> listLocationUserCreate = new ObservableCollection<Models.LocationItem>();

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
            int obj = Convert.ToInt32(objs);
            switch (obj)
            {
                case 0:
                    var vm = new ListLocationViewModel();
                    vm.Mode = 0;
                    await Shell.Current.Navigation.PushAsync(new ListLocationView() { BindingContext = vm });
                    break;
                case 1:
                    var vm1 = new ListLocationViewModel();
                    vm1.Mode = 1;
                    await Shell.Current.Navigation.PushAsync(new ListLocationView() { BindingContext = vm1 });
                    break;
                case 2:
                    var vm2 = new ListLocationViewModel();
                    vm2.Mode = 2;
                    await Shell.Current.Navigation.PushAsync(new ListLocationView() { BindingContext = vm2 });
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
            List<Models.Location> ListGet = await Task.Run(() => _locationService.GetIsShare());
            Task task1 = Task.Run(() =>
            {
                var listnear = ListGet.FindAll(x =>
                                x.Creator == null && x.Province == dataLogin.Instance.currUser.City)
                                .OrderBy(x => x.District == dataLogin.Instance.currUser.Dictrict);
                if (listnear.Count() > 5)
                {
                    var listnew = new ObservableCollection<Models.LocationItem>();
                    foreach (var item in listnear.Take(5))
                    {
                        Models.LocationItem model = new Models.LocationItem();
                        model.LocationCurrent = item;
                        if(dataLogin.Instance.currUser.SaveStore != null)
                        {
                            if(dataLogin.Instance.currUser.SaveStore.Where(x => x == item.Id).FirstOrDefault() != null)
                            {
                                model.IsSave = true;
                            }
                        }
                        listnew.Add(model);
                    }
                    ListLocationNear = listnew;
                }
                else
                {
                    var listnew = new ObservableCollection<Models.LocationItem>();
                    foreach (var item in listnear)
                    {
                        Models.LocationItem model = new Models.LocationItem();
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
                    ListLocationNear = listnew;
                }
            });
            Task task2 = Task.Run(() =>
            {
                var listtag = ListGet.Where(x => x.Tags.Any(value => dataLogin.Instance.currUser.Tags.Contains(value))).ToList();
                if (listtag.Count() > 5)
                {
                    var listnew = new ObservableCollection<Models.LocationItem>();
                    foreach (var item in listtag.Take(5))
                    {
                        Models.LocationItem model = new Models.LocationItem();
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
                    ListLocationByTag = listnew;
                }
                else
                {
                    var listnew = new ObservableCollection<Models.LocationItem>();
                    foreach (var item in listtag)
                    {
                        Models.LocationItem model = new Models.LocationItem();
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
                    ListLocationByTag = listnew;
                }
            });
            Task task3 = Task.Run(() =>
            {
                var listcreate = ListGet.Where(x => x.Creator == dataLogin.Instance.currUser.Id).ToList();
                if (listcreate.Count() > 5)
                {
                    var listnew = new ObservableCollection<Models.LocationItem>();
                    foreach (var item in listcreate.Take(5))
                    {
                        Models.LocationItem model = new Models.LocationItem();
                        model.LocationCurrent = item;
                        listnew.Add(model);
                    }
                    ListLocationUserCreate = listnew;
                }
                else
                {
                    var listnew = new ObservableCollection<Models.LocationItem>();
                    foreach (var item in listcreate)
                    {
                        Models.LocationItem model = new Models.LocationItem();
                        model.LocationCurrent = item;
                        listnew.Add(model);
                    }
                    ListLocationUserCreate = listnew;
                }
            });
            await Task.WhenAll(task1, task2, task3);
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
