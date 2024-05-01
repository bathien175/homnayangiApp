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
        private bool isExecuteCMD = false;
        private bool _isClone = false;

        public Location LocationCurrent { get => _locationCurrent;
            set
            { 
                SetProperty(ref _locationCurrent, value);
                if (value.Creator != null && value.Creator == dataLogin.Instance.currUser.Id)
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
        public DelegateCommand<Location> CloneLocationCommand { get; }
        public DelegateCommand gotoDetail { get; }
        public DelegateCommand TransferCommand { get; }
        public bool IsUserCreate { get => _isUserCreate; set => SetProperty(ref _isUserCreate, value); }
        public bool IsExecuteCMD { get => isExecuteCMD; set => SetProperty(ref isExecuteCMD, value); }
        public bool IsClone { get => _isClone; set => SetProperty(ref _isClone, value); }

        public LocationItem()
        {
            SaveCommand = new DelegateCommand(executeSaveCMD);
            gotoDetail = new DelegateCommand(executeGotoDetailCMD);
            TransferCommand = new DelegateCommand(executeTransferCMD);
            CloneLocationCommand = new DelegateCommand<Location>(executeCloneLocationCMD);
        }

        private async void executeCloneLocationCMD(Location location)
        {
            if (IsExecuteCMD)
                return;

            IsExecuteCMD = true;
            IUserService userService = new UserService();
            if (IsClone)
            {
                await Shell.Current.DisplayAlert("Cảnh báo!","Địa điểm này đã được lưu!","Đã hiểu");
            }
            else
            {
                IsClone = true;
                await Task.Run(() =>
                {
                    if (dataLogin.Instance.currUser.CloneLocation == null)
                    {
                        dataLogin.Instance.currUser.CloneLocation = new List<string>();
                        dataLogin.Instance.currUser.CloneLocation.Add(LocationCurrent.Id);
                        userService.Update(dataLogin.Instance.currUser.Id, dataLogin.Instance.currUser);
                    }
                    else
                    {
                        dataLogin.Instance.currUser.CloneLocation.Add(LocationCurrent.Id);
                        userService.Update(dataLogin.Instance.currUser.Id, dataLogin.Instance.currUser);
                    }
                });
                await Task.Run(() =>
                {
                    ILocationService locationService = new LocationService();
                    Location l = new Location()
                    {
                        Name = location.Name,
                        Address = location.Address,
                        CloseTime = location.CloseTime,
                        District = location.District,
                        HotLine = location.HotLine,
                        Images = location.Images,
                        IsOpen24H = location.IsOpen24H,
                        IsShare = true,
                        MaxPrice = location.MaxPrice,
                        MinPrice = location.MinPrice,
                        OpenTime = location.OpenTime,
                        Province = location.Province,
                        Tags = location.Tags,
                        Creator = dataLogin.Instance.currUser.Id
                    };
                    locationService.Create(l);
                });
            }
            IsExecuteCMD = false;
        }

        private async void executeTransferCMD()
        {
            if (IsExecuteCMD)
                return;
            
            string UserLogin = dataLogin.Instance.currUser.Id;
            Location newlocate = new Location()
            {
                Creator = UserLogin,
                Name = LocationCurrent.Name,
                HotLine = LocationCurrent.HotLine,
                Address = LocationCurrent.Address,
                District = LocationCurrent.District,
                Province = LocationCurrent.Province,
                OpenTime = LocationCurrent.OpenTime,
                CloseTime = LocationCurrent.CloseTime,
                IsOpen24H = LocationCurrent.IsOpen24H,
                MaxPrice = LocationCurrent.MaxPrice,
                MinPrice = LocationCurrent.MinPrice,
                IsShare = true,
                Images = LocationCurrent.Images,
                Tags = LocationCurrent.Tags
            };
            ILocationService locationService = new LocationService();
            await locationService.Create(newlocate);
            await Shell.Current.DisplayAlert("Thành công","Lưu địa điểm thành công","OK");
        }

        private async void executeGotoDetailCMD()
        {
            if (IsExecuteCMD)
                return;

            IsExecuteCMD = true;
            var vm = await Task.Run(() => new DetailLocationViewModel
            {
                LocationCurr = this
            });
            await Shell.Current.Navigation.PushModalAsync(new DetailLocationView() { BindingContext = vm});
            IsExecuteCMD = false;
        }

        private async void executeSaveCMD()
        {
            if (IsExecuteCMD)
                return;

            IsExecuteCMD = true;
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
            IsExecuteCMD = false;
        }
    }
}
