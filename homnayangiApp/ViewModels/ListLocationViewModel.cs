using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using System.Collections.ObjectModel;

namespace homnayangiApp.ViewModels
{
    public class ListLocationViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private ObservableCollection<LocationItem> listLocat = new ObservableCollection<LocationItem>();
        private int mode;
        private bool isViewCreate = false;
        private bool isLoading = false;
        private string titlePage = string.Empty;

        public ObservableCollection<LocationItem> ListLocat { get => listLocat; set => SetProperty(ref listLocat, value); }
        public int Mode { get => mode; set
            {
                SetProperty(ref mode, value);
                if(value == 2)
                {
                    TitlePage = "Địa điểm đã tạo";
                    IsViewCreate = true;
                    loadLocation(value);
                }
                else
                {
                    if (value == 0)
                    {
                        TitlePage = "Địa điểm ở gần";
                    }
                    else
                    {
                        TitlePage = "Địa điểm cùng tags";
                    }
                    IsViewCreate = false;
                    loadLocation(value);
                }
            } 
        }

        private async void loadLocation(int value)
        {
            IsLoading = true;
            switch (mode)
            {
                case 0: // listnear
                    var u = dataLogin.Instance.currUser;
                    List<Models.Location> listget = await Task.Run(() => _locationService.GetNear(u.City, u.Dictrict));
                    var listnew = new ObservableCollection<Models.LocationItem>();
                    await Task.Run(() =>
                    {
                        if (listget.Count > 0)
                        {
                            foreach (var item in listget)
                            {
                                LocationItem model = new LocationItem();
                                model.LocationCurrent = item;
                                if (u.SaveStore != null)
                                {
                                    if (u.SaveStore.Where(x => x == item.Id).FirstOrDefault() != null)
                                    {
                                        model.IsSave = true;
                                    }
                                }
                                listnew.Add(model);
                            }
                        }
                        ListLocat = listnew;
                        IsLoading = false;
                    });
                    break;
                case 1: //listtag
                    var u1 = dataLogin.Instance.currUser;
                    List<Models.Location> listget1 = await Task.Run(() => _locationService.GetByTag(u1.Tags));
                    var listnew1 = new ObservableCollection<Models.LocationItem>();
                    await Task.Run(() =>
                    {
                        if (listget1.Count > 0)
                        {
                            foreach (var item in listget1)
                            {
                                LocationItem model = new LocationItem();
                                model.LocationCurrent = item;
                                if (u1.SaveStore != null)
                                {
                                    if (u1.SaveStore.Where(x => x == item.Id).FirstOrDefault() != null)
                                    {
                                        model.IsSave = true;
                                    }
                                }
                                listnew1.Add(model);
                            }
                        }
                        ListLocat = listnew1;
                        IsLoading = false;
                    });
                    break;
                case 2: //listcreate
                    var u2 = dataLogin.Instance.currUser;
                    List<Models.Location> listget2 = await Task.Run(() => _locationService.GetCreate(u2.Id));
                    var listnew2 = new ObservableCollection<Models.LocationItem>();
                    await Task.Run(() =>
                    {
                        if (listget2.Count > 0)
                        {
                            foreach (var item in listget2)
                            {
                                LocationItem model = new LocationItem();
                                model.LocationCurrent = item;
                                listnew2.Add(model);
                            }
                        }
                        ListLocat = listnew2;
                        IsLoading = false;
                    });
                    break;
            }
        }

        public bool IsViewCreate { get => isViewCreate; set => SetProperty(ref isViewCreate, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public string TitlePage { get => titlePage; set => SetProperty(ref titlePage, value); }

        public DelegateCommand backPage { get; }
        public ListLocationViewModel()
        {
            _locationService = new LocationService();
            backPage = new DelegateCommand(executeBackPageCMD);
        }

        private async void executeBackPageCMD()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
