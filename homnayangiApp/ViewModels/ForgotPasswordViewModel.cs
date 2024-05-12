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
using UraniumUI.Dialogs.Mopups;

namespace homnayangiApp.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private ObservableCollection<FindUserModel> listFilter = new ObservableCollection<FindUserModel>();
        private string textFilter = String.Empty;
        private bool isLoading = false;
        public DelegateCommand searchCMD { get; }
        public ObservableCollection<FindUserModel> ListFilter { get => listFilter; set => SetProperty(ref listFilter, value); }
        public string TextFilter { get => textFilter; 
            set 
            { 
                SetProperty(ref textFilter, value);
            } 
        }
        public DelegateCommand<string> restoreCMD { get; }
        public DelegateCommand BackToLoginCmd { get; }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public ForgotPasswordViewModel()
        {
            _userService = new UserService();
            restoreCMD = new DelegateCommand<string>(executeRestoreCMD);
            searchCMD = new DelegateCommand(executeSearchCMD);
            BackToLoginCmd = new DelegateCommand(executeBackToLoginCMD);
        }

        private async void executeSearchCMD()
        {
            IsLoading = true;
            ObservableCollection<FindUserModel> listnew = [];
            var l = await _userService.SearchUserForgot(TextFilter);
            if (l.Count > 0)
            {
                foreach (var item in l)
                {
                    FindUserModel model = new FindUserModel();
                    model.NameUser = item.Name;
                    model.PhoneRealUser = item.Phone;
                    model.PhoneFakeUser = MaskPhoneNumber(item.Phone);
                    model.ImageString = item.ImageData;
                    model.RestorePassword = restoreCMD;
                    model.ImgUser = item.ImageData;
                    listnew.Add(model);
                }
            }
            ListFilter = listnew;
            IsLoading = false;
        }

        private async void executeBackToLoginCMD()
        {
            await Shell.Current.GoToAsync("//Login");
        }

        private async void executeRestoreCMD(string sdt)
        {
            var result = await Shell.Current.DisplayPromptAsync("Xác nhận số điện thoại!", "Vui lòng nhập đúng với số điện thoại bạn đã đăng ký", placeholder: "VD: 0987654321");
            if (result == sdt)
            {
                IsLoading = true;
                try
                {
                    await _userService.RestorePassword(sdt);
                    await Shell.Current.DisplayAlert("Chúc mừng", "Phục hồi mật khẩu thành công, Bạn có thể tiến hành đăng nhập lại", "OK");
                    IsLoading = false;
                    await Shell.Current.Navigation.PopAsync();
                }
                catch (Exception)
                {
                    await Shell.Current.DisplayAlert("Thất bại", "Server không có phản hồi!", "OK");
                    IsLoading = false;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Thất bại", "Số điện thoại không trùng khớp!", "OK");
                IsLoading = false;
            }
        }
        //private void loadFilter(string s)
        //{
        //    if (s == "")
        //    {
        //        ListFilter.Clear();
        //    }
        //    else
        //    {
        //        ObservableCollection<FindUserModel> listnew = new ObservableCollection<FindUserModel>();
        //        var list2 = new ObservableCollection<FindUserModel>(ListFull.Where(x => x.NameUser.ToLower().Contains(s.ToLower())).ToList());
        //        if (list2.Count > 0)
        //        {
        //            foreach (var item in list2)
        //            {
        //                FindUserModel model = new FindUserModel();
        //                model.PhoneFakeUser = item.PhoneFakeUser;
        //                model.NameUser = item.NameUser;
        //                model.PhoneRealUser = item.PhoneRealUser;
        //                model.RestorePassword = item.RestorePassword;
        //                model.ImgUser = item.ImageString;
        //                listnew.Add(model);
        //            }
        //        }
        //        ListFilter = listnew;
        //    }
        //}
        static string MaskPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length <= 2)
                return phoneNumber; // Trả về chuỗi ban đầu nếu có ít hơn hoặc bằng 2 ký tự

            string maskedString = new string('*', phoneNumber.Length - 2) + phoneNumber.Substring(phoneNumber.Length - 2);
            return maskedString;
        }
    }
}
