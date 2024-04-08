using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private string phone = String.Empty;
        private string name = String.Empty;
        private string password = String.Empty;
        private string repassword = String.Empty;
        private bool _isNoMatchingPassword = false;
        private bool isLoading = false;

        public string Phone { get => phone; set => SetProperty(ref phone, value); }
        public string Name { get => name; set => SetProperty(ref name, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string Repassword
        {
            get => repassword; set
            {
                SetProperty(ref repassword, value);
                if (!value.Equals(Password))
                {
                    IsNoMatchingPassword = true;
                }
                else
                {
                    IsNoMatchingPassword = false;
                }
            }
        }
        public bool IsNoMatchingPassword { get => _isNoMatchingPassword; set => SetProperty(ref _isNoMatchingPassword, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public DelegateCommand BackStepCmd { get; }
        public DelegateCommand SignInCmd { get; }

        public SignInViewModel()
        {
            _userService = new UserService();
            Name = dataSignIn.Instance.userName;
            Phone = dataSignIn.Instance.userPhone;
            Password = dataSignIn.Instance.userPass;
            Repassword = Password;
            SignInCmd = new DelegateCommand(executeSignInCMD);
            BackStepCmd = new DelegateCommand(executeBackStepCMD);
        }

        private async void executeBackStepCMD()
        {
            await Shell.Current.GoToAsync("//Login");
        }

        private async void executeSignInCMD()
        {
            IsLoading = true;
            if (!Repassword.Equals(Password))
            {
                IsNoMatchingPassword = true;
            }
            else
            {
                IsNoMatchingPassword = false;
                var u = await _userService.GetbyPhone(Phone);
                if (u == null)
                {
                    try
                    {
                        dataSignIn.Instance.userName = Name;
                        dataSignIn.Instance.userPhone = Phone;
                        dataSignIn.Instance.userPass = Password;
                        await Shell.Current.GoToAsync("//SignInStep2");
                        IsLoading = false;
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Thất bại!", "Server xảy ra lỗi!", "Thử lại");
                        IsLoading = false;
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Thất bại!", "Số điện thoại đã được đăng ký trước đó!", "Thử lại");
                    IsLoading = false;
                }
            }
        }
    }
}
