using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ViewModels
{
    public class CommunityViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        //private
        private bool isLoading = false;
        private ObservableCollection<User> listUser = new ObservableCollection<User>();
        private string textSearch = string.Empty;
        //command
        public DelegateCommand backPage { get; }
        public DelegateCommand searchUser { get; }
        //public
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public ObservableCollection<User> ListUser { get => listUser; set => SetProperty(ref listUser, value); }
        public string TextSearch { get => textSearch; set => SetProperty(ref textSearch, value); }

        public CommunityViewModel() 
        {
            _userService = new UserService();
            backPage = new DelegateCommand(executeBackPageCMD);
            searchUser = new DelegateCommand(executeSearchCMD);
        }

        private async void executeSearchCMD()
        {
            if(TextSearch == string.Empty)
            {
                ListUser.Clear();
            }
            else
            {
                IsLoading = true;
                var a = await Task.Run(() => _userService.SearchUser(TextSearch, dataLogin.Instance.currUser.IDUser));
                if(a.Count == 0)
                {
                    ListUser.Clear();
                }
                else
                {
                    ListUser = new ObservableCollection<User>(a);
                }
                IsLoading = false;
            }
        }

        private async void executeBackPageCMD()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
