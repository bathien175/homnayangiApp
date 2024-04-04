using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using System.Collections.ObjectModel;

namespace homnayangiApp.ViewModels
{
    public class DetailLocationViewModel: BaseViewModel
    {
        private readonly ILocationService _locationService;
        private ObservableCollection<string?> listImages = new ObservableCollection<string?>();
        private ObservableCollection<string> listTags = new ObservableCollection<string>();
        private Timer timer;
        private int currentIndex = 0;
        private bool isLoading = false;
        //private string nameLocation = string.Empty;
        private Models.LocationItem locationCurr = new Models.LocationItem();
        //private long minPrice;
        //private long maxPrice;
        //private string openTime = string.Empty;
        //private string closeTime = string.Empty;
        //private bool isOpen24H = false;
        //private string phone = string.Empty;
        private string address = string.Empty;
        private string type = string.Empty;

        public ObservableCollection<string?> ListImages { get => listImages; set => SetProperty(ref listImages, value); }
        public Timer Timer { get => timer; set => SetProperty(ref timer, value); }
        public int CurrentIndex { get => currentIndex; set => SetProperty(ref currentIndex, value); }
        public ObservableCollection<string> ListTags { get => listTags; set => SetProperty(ref listTags, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        //public string NameLocation { get => nameLocation; set => SetProperty(ref nameLocation, value); }
        public Models.LocationItem LocationCurr { get => locationCurr; set
            {
                SetProperty(ref locationCurr, value);
                loadData();
            }
        }
        //public long MinPrice { get => minPrice; set => SetProperty(ref minPrice, value); }
        //public long MaxPrice { get => maxPrice; set => SetProperty(ref maxPrice, value); }
        //public string OpenTime { get => openTime; set => SetProperty(ref openTime, value); }
        //public string CloseTime { get => closeTime; set => SetProperty(ref closeTime, value); }
        //public bool IsOpen24H { get => isOpen24H; set => SetProperty(ref isOpen24H, value); }
        //public string Phone { get => phone; set => SetProperty(ref phone, value); }
        public string Address { get => address; set => SetProperty(ref address, value); }
        public string Type { get => type; set => SetProperty(ref type, value); }


        public DelegateCommand BackPage { get; }

        public DetailLocationViewModel()
        {
            BackPage = new DelegateCommand(executeBackPageCMD);
        }

        private async void executeBackPageCMD()
        {
            await Shell.Current.Navigation.PopAsync(true);
        }

        public async void loadData()
        {
            IsLoading = true;
            await Task.Run(() =>
            {
                //NameLocation = LocationCurr.LocationCurrent.Name;
                Address = $"{LocationCurr.LocationCurrent.Address}, {LocationCurr.LocationCurrent.District}, {LocationCurr.LocationCurrent.Province}";
                if (LocationCurr.LocationCurrent.Creator != null)
                {
                    if (LocationCurr.LocationCurrent.Creator == dataLogin.Instance.currUser.Id)
                    {
                        Type = "Địa điểm tự tạo";
                    }
                    else
                    {
                        Type = "Địa điểm người khác tạo";
                    }
                }
                else
                {
                    Type = "Địa điểm có sẵn";
                }

                //if (LocationCurr.LocationCurrent.IsOpen24H == true)
                //{
                //    OpenTime = "24";
                //    CloseTime = "24";
                //}
                //else
                //{
                //    OpenTime = LocationCurr.LocationCurrent.OpenTime.Value.ToString(@"hh\:mm");
                //    CloseTime = LocationCurr.LocationCurrent.CloseTime.Value.ToString(@"hh\:mm");
                //}
                //MinPrice = LocationCurr.LocationCurrent.MinPrice;
                //MaxPrice = LocationCurr.LocationCurrent.MaxPrice;
                //Phone = LocationCurr.LocationCurrent.HotLine;
                if (LocationCurr.LocationCurrent.Images == null)
                {
                    ListImages.Add(null);
                }
                else
                {
                    ListImages = new ObservableCollection<string?>(LocationCurr.LocationCurrent.Images);
                }
                if (ListImages.Count > 1)
                {
                    var timer = Application.Current.Dispatcher.CreateTimer();
                    timer.Interval = TimeSpan.FromSeconds(5);
                    timer.Tick += (s, e) => DoSomething();
                    timer.Start();
                }
                ListTags = new ObservableCollection<string>(LocationCurr.LocationCurrent.Tags);
                IsLoading = false;
            });
        }
        private void DoSomething()
        {
            CurrentIndex = (CurrentIndex + 1) % ListImages.Count;
        }
    }
}
