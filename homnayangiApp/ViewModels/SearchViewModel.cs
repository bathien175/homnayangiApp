using CommunityToolkit.Maui.Views;
using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ViewModels
{
    [QueryProperty(nameof(TextSearchByMainPage), nameof(TextSearchByMainPage))]
    public class SearchViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private List<string> searchHistory = [];
        private List<string> searchHistoryReverse = [];
        private ObservableCollection<LocationItem> listLocation = [];
        private ObservableCollection<LocationItem> listLocationFilter = [];
        private bool isLoading = false;
        private bool isSearching = false;
        private string textSearch = string.Empty;
        private string textSearchByMainPage = string.Empty;
        private string selectItem = string.Empty;
        private bool isFilter = false;
        private List<string> listTag = new List<string>();
        private ObservableCollection<string> tagSelect = new ObservableCollection<string>();
        private List<Province> listCity = dataCity.Instance.listProvince;
        private List<String> listDistrict = dataCity.Instance.listDistrict;
        private int citySelect;
        private string districtSelect = string.Empty;
        private bool isFilterByTag = false;
        private bool isFilterByProvince = false;
        private bool isFilterByDistrict = false;
        public DelegateCommand ShowPopupCMD { get; }
        public DelegateCommand FilterCMD { get; }
        public DelegateCommand SearchLocationCMD { get; }
        public DelegateCommand ResetLocationCMD { get; }
        public DelegateCommand<string> SelectHistoryCMD { get; }
        public DelegateCommand BackPage { get; }
        public List<string> SearchHistory { get => searchHistory; 
            set 
            { 
                SetProperty(ref searchHistory, value);
                var list = new List<string>(value);
                if (list.Count() > 0)
                {
                    list.Reverse();
                    SearchHistoryReverse = list;
                }
            } 
        }
        public List<string> SearchHistoryReverse { get => searchHistoryReverse; set => SetProperty(ref searchHistoryReverse, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public bool IsSearching { get => isSearching; set => SetProperty(ref isSearching, value); }
        public string TextSearch { get => textSearch; set => SetProperty(ref textSearch, value); }
        public ObservableCollection<LocationItem> ListLocation { get => listLocation;
            set
            {
                SetProperty(ref listLocation, value);
                filterResult();
            }
        }
        public ObservableCollection<LocationItem> ListLocationFilter { get => listLocationFilter;
            set
            {
                SetProperty(ref listLocationFilter, value);
            }
        }
        public string SelectItem { get => selectItem; 
            set 
            { 
                SetProperty(ref selectItem, value); 
                if(value!= string.Empty)
                {
                    executeSelectHistoryCMD(value);
                }
            } 
        }
        public string TextSearchByMainPage { get => textSearchByMainPage;
            set
            {
                SetProperty(ref textSearchByMainPage, value);
                TextSearch = value;
                executeSearchCMD();
            }
        }
        public bool IsFilter { get => isFilter; set => SetProperty(ref isFilter, value); }
        public List<string> ListTag { get => listTag; set => SetProperty(ref listTag, value); }
        public ObservableCollection<string> TagSelect { get => tagSelect; set => SetProperty(ref tagSelect, value); }
        public List<Province> ListCity { get => listCity; set => SetProperty(ref listCity, value); }
        public List<string> ListDistrict { get => listDistrict; set => SetProperty(ref listDistrict, value); }
        public int CitySelect { get => citySelect; set => SetProperty(ref citySelect, value); }
        public string DistrictSelect { get => districtSelect; set => SetProperty(ref districtSelect, value); }
        public bool IsFilterByTag { get => isFilterByTag; set => SetProperty(ref isFilterByTag, value); }
        public bool IsFilterByProvince { get => isFilterByProvince; set => SetProperty(ref isFilterByProvince, value); }
        public bool IsFilterByDistrict { get => isFilterByDistrict; set => SetProperty(ref isFilterByDistrict, value); }

        public SearchViewModel()
        {
            _locationService = new LocationService();
            GetSearchHistory();
            loadTag();
            DistrictSelect = ListDistrict[0];
            SearchLocationCMD = new DelegateCommand(executeSearchCMD);
            ResetLocationCMD = new DelegateCommand(executeResetLocationCMD);
            BackPage = new DelegateCommand(executeBackPageCMD);
            SelectHistoryCMD = new DelegateCommand<string>(executeSelectHistoryCMD);
            ShowPopupCMD = new DelegateCommand(executeShowPopupCMD);
            FilterCMD = new DelegateCommand(executeFilterCMD);
        }

        private async void loadTag()
        {
            await Task.Run(async () =>
            {
                ITagsService _tags = new TagsService();
                var a = await _tags.Get();
                List<string> listnew = [];
                if (a.Count > 0)
                {
                    foreach (var item in a)
                    {
                        listnew.Add(item.Name);
                    }
                }
                ListTag = listnew.OrderBy(s => s).ToList();
            });
        }

        private void executeFilterCMD()
        {
            executeSearchCMD();
        }

        private void executeShowPopupCMD()
        {
            IsFilter = !IsFilter;
        }

        private async void executeBackPageCMD()
        {
            await Shell.Current.GoToAsync("//HomeApp/MainPage");
        }

        private void executeSelectHistoryCMD(string t)
        {
            TextSearch = t;
            executeSearchCMD();
        }

        private void executeResetLocationCMD()
        {
            TextSearch = string.Empty;
            SelectItem = string.Empty;
            IsSearching = false;
            ListLocation = [];
        }

        private async void executeSearchCMD()
        {
            if (TextSearch == string.Empty)
                return;

            IsLoading = true;
            IsSearching = true;
            if (SearchHistory.Where(x => x == TextSearch).FirstOrDefault() == null)
            {
                var newlist = new List<string>(SearchHistory)
                {
                    TextSearch
                };
                SearchHistory = newlist;
                var listreverse = new List<string>(SearchHistoryReverse.Take(10));
                listreverse.Reverse();
                await SaveSearchHistory(listreverse);
            }
            List<Models.Location> listget = await Task.Run(() => _locationService.GetIsShare());
            await Task.Run(() =>
            {
                var listfind = listget.Where(x => x.Name.ToLower().Contains(TextSearch.ToLower())).ToList();
                ObservableCollection<LocationItem> listnew = [];
                if(listfind.Count > 0)
                {
                    foreach (var item in listfind)
                    {
                        LocationItem model = new LocationItem
                        {
                            LocationCurrent = item
                        };
                        // Kiểm tra xem địa điểm này có nằm trong danh sách lưu trữ của người dùng hay không
                        if (dataLogin.Instance.currUser.SaveStore != null)
                        {
                            if (dataLogin.Instance.currUser.SaveStore.Contains(item.Id))
                            {
                                model.IsSave = true;
                            }
                        }
                        listnew.Add(model);
                    }
                }
                ListLocation = listnew;
                IsLoading = false;
            });
        }
        void filterResult()
        {
            if (IsFilterByTag)
            {
                if (IsFilterByProvince)
                {
                    if (IsFilterByDistrict)
                    {
                        //cả 3 có
                        ListLocationFilter = new ObservableCollection<LocationItem>(ListLocation.Where(x => x.LocationCurrent.Province == ListCity[CitySelect].province_name
                                                           && x.LocationCurrent.District == DistrictSelect
                                                           && x.LocationCurrent.Tags.Any(tag => TagSelect.Contains(tag))).ToList());
                    }
                    else
                    {
                        //không có huyện
                        ListLocationFilter = new ObservableCollection<LocationItem>(ListLocation.Where(x => x.LocationCurrent.Province == ListCity[CitySelect].province_name
                                   && x.LocationCurrent.Tags.Any(tag => TagSelect.Contains(tag))).ToList());
                    }
                }
                else
                {
                    if (IsFilterByDistrict)
                    {
                        //không có thành phố
                        ListLocationFilter = new ObservableCollection<LocationItem>(ListLocation.Where(x => x.LocationCurrent.District == DistrictSelect
                                   && x.LocationCurrent.Tags.Any(tag => TagSelect.Contains(tag))).ToList());
                    }
                    else
                    {
                        //không có thành phố, quận huyện
                        ListLocationFilter = new ObservableCollection<LocationItem>(ListLocation.Where(x => x.LocationCurrent.Tags.Any(tag => TagSelect.Contains(tag))).ToList());
                    }
                }
            }
            else
            {
                if (IsFilterByProvince)
                {
                    if (IsFilterByDistrict)
                    {
                        //không có tag
                        ListLocationFilter = new ObservableCollection<LocationItem>(ListLocation.Where(x => x.LocationCurrent.Province == ListCity[CitySelect].province_name
                                   && x.LocationCurrent.District == DistrictSelect));
                    }
                    else
                    {
                        //không có tag và quận huyện
                        ListLocationFilter = new ObservableCollection<LocationItem>(ListLocation.Where(x => x.LocationCurrent.Province == ListCity[CitySelect].province_name));
                    }
                }
                else
                {
                    if (IsFilterByDistrict)
                    {
                        //không có tag và thành phố
                        ListLocationFilter = new ObservableCollection<LocationItem>(ListLocation.Where(x => x.LocationCurrent.District == DistrictSelect));
                    }
                    else
                    {
                        //không có cả 3 -> trả list gốc
                        ListLocationFilter = new ObservableCollection<LocationItem>(ListLocation);
                    }
                }
            }
        }
        // Lưu trữ lịch sử tìm kiếm vào tệp tin
        public async Task SaveSearchHistory(List<string> searchHistory)
        {
            string json = JsonConvert.SerializeObject(searchHistory);
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{dataLogin.Instance.currUser.IDUser}.json");
            await File.WriteAllTextAsync(filePath, json);
        }

        // Lấy lịch sử tìm kiếm từ tệp tin
        public async void GetSearchHistory()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{dataLogin.Instance.currUser.IDUser}.json");
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                SearchHistory = JsonConvert.DeserializeObject<List<string>>(json) ?? [];
            }
            else
            {
                SearchHistory = [];
            }
        }
    }
}
