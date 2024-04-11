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
    public class SaveLocationViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private bool isLoading = false;
        private ObservableCollection<LocationItem> listLocat = new ObservableCollection<LocationItem>();
        private ObservableCollection<LocationItem> listLocatFilter = new ObservableCollection<LocationItem>();
        private string textFilter = string.Empty;

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public ObservableCollection<LocationItem> ListLocat { get => listLocat;
            set
            {
                SetProperty(ref listLocat, value);
                filterList();
            }
        }
        public ObservableCollection<LocationItem> ListLocatFilter { get => listLocatFilter; set => SetProperty(ref listLocatFilter, value); }
        public string TextFilter { get => textFilter;
            set
            {
                SetProperty(ref textFilter, value);
                filterList();
            }
        }

        public SaveLocationViewModel()
        {
            _locationService = new LocationService();
        }

        public async void LoadData()
        {
            var u = dataLogin.Instance.currUser;
            if(u.SaveStore!=null)
            {
                if(u.SaveStore.Count > 0)
                {
                    IsLoading = true;
                    List<Models.Location> listget = await Task.Run(() => _locationService.GetSaveLocation(u.SaveStore));
                    ObservableCollection<LocationItem> listnew = [];
                    await Task.Run(() =>
                    {
                        if (listget.Count > 0)
                        {
                            foreach (var item in listget)
                            {
                                LocationItem model = new LocationItem();
                                model.LocationCurrent = item;
                                model.IsSave = true;
                                listnew.Add(model);
                            }
                        }
                        ListLocat = listnew;
                        IsLoading = false;
                    });
                }
            }
        }

        void filterList()
        {
            ListLocatFilter.Clear();
            if (TextFilter==string.Empty)
            {
                ListLocatFilter = new ObservableCollection<LocationItem>(ListLocat);
            }
            else
            {
                ListLocatFilter = new ObservableCollection<LocationItem>(ListLocat.Where(x => x.LocationCurrent.Name.Contains(TextFilter)));
            }
        }
    }
}
