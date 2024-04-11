using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ViewModels
{
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
        private string selectItem = string.Empty;
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

        public SearchViewModel()
        {
            _locationService = new LocationService();
            GetSearchHistory();
            SearchLocationCMD = new DelegateCommand(executeSearchCMD);
            ResetLocationCMD = new DelegateCommand(executeResetLocationCMD);
            BackPage = new DelegateCommand(executeBackPageCMD);
            SelectHistoryCMD = new DelegateCommand<string>(executeSelectHistoryCMD);
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
                var listfind = listget.Where(x => x.Name.ToLower().Contains(TextSearch)).ToList();
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
            ListLocationFilter = ListLocation;
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
