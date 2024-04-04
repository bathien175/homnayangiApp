using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.Models
{
    public class LocationItem : BaseViewModel
    {
        private Location _locationCurrent = new Location();
        private bool _isSave = false;
        private bool _isUserCreate = false;

        public Location LocationCurrent { get => _locationCurrent;
            set
            { 
                SetProperty(ref _locationCurrent, value); 
                if(value.Creator != null)
                {
                    IsUserCreate = true;
                }
            } 
        }
        public bool IsSave { get => _isSave; set
            {
                SetProperty(ref _isSave, value);
            }
        }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand gotoDetail { get; }
        public bool IsUserCreate { get => _isUserCreate; set => SetProperty(ref _isUserCreate, value); }

        public LocationItem()
        {
            SaveCommand = new DelegateCommand(executeSaveCMD);
            gotoDetail = new DelegateCommand(executeGotoDetailCMD);
        }

        private async void executeGotoDetailCMD()
        {
            var vm = new DetailLocationViewModel();
            vm.LocationCurr = LocationCurrent;
            await Shell.Current.Navigation.PushAsync(new DetailLocationView() { BindingContext = vm});
        }

        private async void executeSaveCMD()
        {
            IUserService userService = new UserService();
            if (IsSave)
            {
                IsSave = false;
                await Task.Run(() =>
                {
                    dataLogin.Instance.currUser.SaveStore.Remove(LocationCurrent.Id);
                    userService.Update(dataLogin.Instance.currUser.Id,dataLogin.Instance.currUser);
                });
            }
            else
            {
                IsSave = true;
                await Task.Run(() =>
                {
                    if(dataLogin.Instance.currUser.SaveStore == null)
                    {
                        dataLogin.Instance.currUser.SaveStore = new List<string>();
                        dataLogin.Instance.currUser.SaveStore.Add(LocationCurrent.Id);
                        userService.Update(dataLogin.Instance.currUser.Id, dataLogin.Instance.currUser);
                    }
                    else
                    {
                        dataLogin.Instance.currUser.SaveStore.Add(LocationCurrent.Id);
                        userService.Update(dataLogin.Instance.currUser.Id, dataLogin.Instance.currUser);
                    }
                });
            }
        }
    }
}
