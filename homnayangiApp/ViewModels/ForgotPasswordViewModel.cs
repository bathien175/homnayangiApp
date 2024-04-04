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
        private ObservableCollection<FindUserModel> listFull = new ObservableCollection<FindUserModel>();
        private ObservableCollection<FindUserModel> listFilter = new ObservableCollection<FindUserModel>();
        private string textFilter = String.Empty;

        public ObservableCollection<FindUserModel> ListFull { get => listFull; set => SetProperty(ref listFull, value); }
        public ObservableCollection<FindUserModel> ListFilter { get => listFilter; set => SetProperty(ref listFilter, value); }
        public string TextFilter { get => textFilter; 
            set 
            { 
                SetProperty(ref textFilter, value);
                loadFilter(value);
            } 
        }
        public DelegateCommand<string> restoreCMD { get; }
        public DelegateCommand BackToLoginCmd { get; }

        public ForgotPasswordViewModel()
        {
            _userService = new UserService();
            restoreCMD = new DelegateCommand<string>(executeRestoreCMD);
            loadData();
            BackToLoginCmd = new DelegateCommand(executeBackToLoginCMD);
        }

        private async void executeBackToLoginCMD()
        {
            await Application.Current.MainPage.Navigation.PopAsync(true);
        }

        private void loadData()
        {
            ListFull.Clear();
            var l = _userService.Get();
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
                    ListFull.Add(model);
                }
            }
        }

        private async void executeRestoreCMD(string sdt)
        {
            var result = await Application.Current.MainPage.DisplayTextPromptAsync("Xác nhận số điện thoại!", "Vui lòng nhập đúng với số điện thoại bạn đã đăng ký", placeholder: "VD: 0987654321");
            if (result == sdt)
            {
                try
                {
                    _userService.RestorePassword(sdt);
                    await Application.Current.MainPage.DisplayAlert("Chúc mừng", "Phục hồi mật khẩu thành công, Bạn có thể tiến hành đăng nhập lại", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync(true);
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Thất bại", "Server không có phản hồi!", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Thất bại", "Số điện thoại không trùng khớp!", "OK");
            }
        }
        private void loadFilter(string s)
        {
            if (s == "")
            {
                ListFilter.Clear();
            }
            else
            {
                ObservableCollection<FindUserModel> listnew = new ObservableCollection<FindUserModel>();
                var list2 = new ObservableCollection<FindUserModel>(ListFull.Where(x => x.NameUser.Contains(s)).ToList());
                if (list2.Count > 0)
                {
                    foreach (var item in list2)
                    {
                        FindUserModel model = new FindUserModel();
                        model.PhoneFakeUser = item.PhoneFakeUser;
                        model.NameUser = item.NameUser;
                        model.PhoneRealUser = item.PhoneRealUser;
                        model.RestorePassword = item.RestorePassword;
                        var imgt = Convert.FromBase64String(item.ImageString);
                        MemoryStream stream2 = new(imgt);
                        ImageSource image = ImageSource.FromStream(() => stream2);
                        model.ImgUser = image;
                        listnew.Add(model);
                    }
                }
                ListFilter = listnew;
            }
        }
        static string MaskPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length <= 2)
                return phoneNumber; // Trả về chuỗi ban đầu nếu có ít hơn hoặc bằng 2 ký tự

            string maskedString = new string('*', phoneNumber.Length - 2) + phoneNumber.Substring(phoneNumber.Length - 2);
            return maskedString;
        }
    }
}
