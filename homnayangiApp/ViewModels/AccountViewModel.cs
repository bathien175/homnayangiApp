
using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using System.Security.Cryptography;
using System.Text;

namespace homnayangiApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private string phone = String.Empty;
        private string name = String.Empty;
        private string password = String.Empty;
        private string repassword = String.Empty;
        private bool _isNoMatchingPassword = false;
        private string phoneLogin = string.Empty;
        private string passLogin = string.Empty;
        private bool isRemember = false;
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
        


        public DelegateCommand SignInCmd { get; }
        public DelegateCommand GotoSignInCmd { get; }
        public DelegateCommand GotoForgotCmd { get; }
        public DelegateCommand CompleteSignInCmd { get; }
        public DelegateCommand BackStepCmd { get; }
        public DelegateCommand BackToLoginCmd { get; }
        public DelegateCommand LoginCmd { get; }
        public string PhoneLogin { get => phoneLogin; set => SetProperty(ref phoneLogin, value); }
        public string PassLogin { get => passLogin; set => SetProperty(ref passLogin, value); }
        public bool IsRemember { get => isRemember; set => SetProperty(ref isRemember, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public AccountViewModel()
        {
            _userService = new UserService();
            //IsRemember = Preferences.Get("RememberLogin",false);
            //if (IsRemember)
            //{
            //    PhoneLogin = Preferences.Get("PhoneNumber", string.Empty);
            //    PassLogin = Preferences.Get("Password", string.Empty);
            //}
            Name = dataSignIn.Instance.userName;
            Phone = dataSignIn.Instance.userPhone;
            Password = dataSignIn.Instance.userPass;
            Repassword = Password;
            BackToLoginCmd = new DelegateCommand(executeBackLoginCMD);
            SignInCmd = new DelegateCommand(executeSignInCMD);
            BackStepCmd = new DelegateCommand(executeBackStepCMD);
            CompleteSignInCmd = new DelegateCommand(executeCompleteSignInCmdCMD);
            GotoSignInCmd = new DelegateCommand(executeGotoSignInCMD);
            GotoForgotCmd = new DelegateCommand(executeGotoForgotCMD);
            LoginCmd = new DelegateCommand(executeLoginCMD);
        }

        private async void executeLoginCMD()
        {
            IsLoading = true;
            try
            {
                var a = await Task.Run(() => _userService.Login(PhoneLogin, PassLogin));
                if (a != null)
                {
                    //if (IsRemember)
                    //{
                    //    Preferences.Set("PhoneNumber", PhoneLogin);
                    //    Preferences.Set("Password", PassLogin);
                    //    Preferences.Set("RememberLogin", true);
                    //}
                    //else
                    //{
                    //    Preferences.Remove("PhoneNumber", PhoneLogin);
                    //    Preferences.Remove("Password", PassLogin);
                    //    Preferences.Set("RememberLogin", false);
                    //}
                    dataLogin.Instance.currUser = a;
                    if (a.IDUser == "@admin@")
                    {
                        var view = new AdminAddLocationView();
                        await Application.Current.MainPage.Navigation.PushAsync(view);
                    }
                    else
                    {
                        var view = new AppShell();
                        Application.Current.MainPage = view;
                    }
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Lỗi đăng nhập", "Số điện thoại hoặc mật khẩu không đúng!", "Thử lại");
                }
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Lỗi đăng nhập", "Server không có phản hồi!", "Thử lại");
            }
            finally { IsLoading = false; }
        }

        private async void executeGotoForgotCMD()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ForgotPasswordView());
        }

        private async void executeBackLoginCMD()
        {
            dataSignIn.Instance.clearData();
            await Application.Current.MainPage.Navigation.PopAsync(true);
        }

        private async void executeBackStepCMD()
        {
            await Application.Current.MainPage.Navigation.PopAsync(true);
        }

        private async void executeCompleteSignInCmdCMD()
        {
            //quay về login
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoginView());

        }

        private async void executeGotoSignInCMD()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SignInView());
        }

        private async void executeSignInCMD()
        {
            if (!Repassword.Equals(Password))
            {
                IsNoMatchingPassword = true;
            }
            else
            {
                IsNoMatchingPassword = false;
                var u = _userService.GetbyPhone(Phone);
                if (u == null)
                {
                    try
                    {
                        dataSignIn.Instance.userName = Name;
                        dataSignIn.Instance.userPhone = Phone;
                        dataSignIn.Instance.userPass = Password;
                        await Application.Current.MainPage.Navigation.PushAsync(new SignInStep2View());
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Thất bại!", "Server xảy ra lỗi!", "Thử lại");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Thất bại!", "Số điện thoại đã được đăng ký trước đó!", "Thử lại");
                }
            }
        }
        public string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
        public byte[] ToByteArrayAsync(ImageSource imageSource)
        {
            try
            {
                // Tạo một Image từ ImageSource
                var image = new Image { Source = imageSource };

                // Lấy đường dẫn của hình ảnh từ ImageSource
                string imagePath = ((FileImageSource)image.Source).File;

                // Mở một luồng từ đường dẫn tệp
                using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    // Tạo một MemoryStream để lưu trữ dữ liệu
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Sao chép dữ liệu từ FileStream vào MemoryStream
                        fs.CopyToAsync(ms);

                        // Trả về mảng byte
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi chuyển đổi ImageSource thành byte array: " + ex.Message);
                return null;
            }
        }
    }
}
