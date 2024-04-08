
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
        private bool _isNoMatchingPassword = false;
        private string phoneLogin = string.Empty;
        private string passLogin = string.Empty;
        private bool isRemember = false;
        private bool isLoading = false;

        public bool IsNoMatchingPassword { get => _isNoMatchingPassword; set => SetProperty(ref _isNoMatchingPassword, value); }

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
            BackToLoginCmd = new DelegateCommand(executeBackLoginCMD);
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
                        await Shell.Current.GoToAsync(nameof(AdminAddLocationView));
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("//HomeApp");
                    }
                }
                else
                {
                   await Shell.Current.DisplayAlert("Lỗi đăng nhập", "Số điện thoại hoặc mật khẩu không đúng!", "Thử lại");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                await Shell.Current.DisplayAlert("Lỗi đăng nhập", "Server không có phản hồi!", "Thử lại");
            }
            finally { IsLoading = false; }
        }

        private async void executeGotoForgotCMD()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//ForgotPassword");
            IsLoading = false;
        }

        private async void executeBackLoginCMD()
        {
            dataSignIn.Instance.clearData();
            await Shell.Current.GoToAsync("//Login");
        }

        private async void executeBackStepCMD()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void executeCompleteSignInCmdCMD()
        {
            //quay về login
            await Shell.Current.GoToAsync("//Login");
        }

        private async void executeGotoSignInCMD()
        {
            IsLoading = true;
            try
            {
                await Shell.Current.GoToAsync("//SignIn");
            }
            catch (Exception ex)
            {

                throw;
            }
            IsLoading = false;
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
