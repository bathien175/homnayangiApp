using Firebase.Auth;
using homnayangiApp.Commands;
using homnayangiApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.Models
{
    public class UserCustomModel : BaseViewModel
    {
        private User currentUser = new User();
        private DelegateCommand<User>? _gotoDetail;

        public User CurrentUser { get => currentUser; set => SetProperty(ref currentUser, value); }
        public DelegateCommand<User>? gotoDetail { get => _gotoDetail; set => SetProperty(ref _gotoDetail, value); }

        public UserCustomModel() { }
    }
}
