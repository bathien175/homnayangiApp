using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ViewModels
{
    public class AccountManagerViewModel : BaseViewModel
    {
        private string textError = string.Empty;
        private readonly IUserService _userService;
        private ImageSource _imageUser;
        private string? _imagestringUser;
        private User curentUser = new User();
        private string nameUser = string.Empty;
        private string genderUser = string.Empty;
        private DateTime datebirthUser = DateTime.Now;
        private string cityUser = string.Empty;
        private string districtUser = string.Empty;
        private List<Province> listCity = dataCity.Instance.listProvince;
        private List<String> listDistrict = dataCity.Instance.listDistrict;
        private int citySelect;
        private List<string> listTag = new List<string>();
        private string oldPass = string.Empty;
        private string newPass = string.Empty;
        private string rePass = string.Empty;
        private string erroroldPass = string.Empty;
        private string errornewPass = string.Empty;
        private string errorRePass = string.Empty;
        private ObservableCollection<string> tagSelect = new ObservableCollection<string>();

        public List<string> ListTag { get => listTag; set => SetProperty(ref listTag, value); }
        public ObservableCollection<string> TagSelect { get => tagSelect;
            set 
            { 
                SetProperty(ref tagSelect, value);
            }
        }
        public List<String> ListGender { get; }
        public DelegateCommand logOutCMD { get; }
        public DelegateCommand gotoSettingCMD { get; }
        public DelegateCommand gotoInformationCMD { get; }
        public DelegateCommand gotoChangePasswordCMD { get; }
        public DelegateCommand BackPageCMD { get; }
        public DelegateCommand TakePic { get; }
        public DelegateCommand ChoosePic { get; }
        public DelegateCommand ResetPic { get; }
        public DelegateCommand UpdateInformation { get; }
        public DelegateCommand ChangePassCMD { get; }
        public AccountManagerViewModel()
        {
            _userService = new UserService();
            CurentUser = dataLogin.Instance.currUser;
            loadTag();
            TagSelect =new ObservableCollection<string>(CurentUser.Tags);
            ListGender = new List<string> { "Nam", "Nữ", "Khác" };
            NameUser = CurentUser.Name;
            GenderUser = CurentUser.Gender;
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime result = DateTime.ParseExact(CurentUser.DateBirth, "dd-MM-yyyy", provider);
            DatebirthUser = result;
            CitySelect = ListCity.IndexOf(ListCity.Where(x => x.province_name == CurentUser.City).First());
            DistrictUser = CurentUser.Dictrict;
            logOutCMD = new DelegateCommand(executeLogoutCMD);
            gotoSettingCMD = new DelegateCommand(executeSettingsCMD);
            BackPageCMD = new DelegateCommand(executeBackPageCMD);
            TakePic = new DelegateCommand(executeTakePicCMD);
            ChoosePic = new DelegateCommand(executeChoosePicCMD);
            ResetPic = new DelegateCommand(executeResetPicCMD);
            UpdateInformation = new DelegateCommand(executeUpdateInfoCMD);
            gotoChangePasswordCMD = new DelegateCommand(executeGotoChangePassCMD);
            ChangePassCMD = new DelegateCommand(executeChangePassCMD);
            gotoInformationCMD = new DelegateCommand(executegotoInformationCMD);
        }

        private async void executegotoInformationCMD()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AccountManagerView());
        }

        private async void executeChangePassCMD()
        {
            checkOldPass(OldPass);
            checkNewPass(NewPass);
            checkReNewPass(RePass);
            if(ErroroldPass==string.Empty && ErrornewPass==string.Empty && ErrorRePass == string.Empty)
            {
                User u = CurentUser;
                u.Password = GetMD5Hash(NewPass);
                _userService.Update(u.Id,u);
                dataLogin.Instance.currUser = u;
                CurentUser = u;
                await Application.Current.MainPage.DisplayAlert("Thành công","Đổi mật khẩu thành công","OK");
                await Application.Current.MainPage.Navigation.PopAsync(true);
            }
        }

        private async void executeGotoChangePassCMD()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ChangePasswordView());
        }

        private void loadTag()
        {
            ListTag.Clear();
            ITagsService _tags = new TagsService();
            var a = _tags.Get();
            if (a.Count > 0)
            {
                foreach (var item in a)
                {
                    ListTag.Add(item.Name);
                }
            }
        }

        private async void executeUpdateInfoCMD()
        {
            if (TagSelect.Count == 0)
            {
                TextError = "Không được bỏ trống tags";
            }
            else if (TagSelect.Count > 5)
            {
                TextError = "Không được chọn nhiều hơn 5 tag";
            }
            else
            {
                TextError = string.Empty;
                User u = new User();
                u.Id = CurentUser.Id;
                u.Phone = CurentUser.Phone;
                u.IDUser = CurentUser.IDUser;
                u.Name = NameUser;
                u.Gender = GenderUser;
                u.DateBirth = DatebirthUser.ToString("dd-MM-yyyy");
                u.Password = CurentUser.Password;
                u.City = CityUser;
                u.Dictrict = DistrictUser;
                u.SaveStore = CurentUser.SaveStore;
                u.Tags = new List<string>(TagSelect);
                u.ImageData = ImagestringUser;
                try
                {
                    _userService.Update(u.Id, u);
                    dataLogin.Instance.currUser = u;
                    CurentUser = u;
                    await Application.Current.MainPage.DisplayAlert("Thành công","Cập nhật thông tin cá nhân thành công","OK");
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Thất bại", "Server xảy ra lỗi trong quá trình đọc dữ liệu", "Thử lại");
                }
            }
        }

        private void executeResetPicCMD()
        {
            if (CurentUser.ImageData != null)
            {
                ImagestringUser = CurentUser.ImageData;
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(curentUser.ImageData));
                ImageSource image = ImageSource.FromStream(() => stream);
                ImageUser = image;
            }
            else
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "noimage.png");
                ImagestringUser = null;
                ImageUser = ImageSource.FromFile(imagePath);
            }
        }

        private async void executeChoosePicCMD()
        {
            var mediaFile = await MediaPicker.PickPhotoAsync();
            if (mediaFile != null)
            {
                string imageformat = string.Empty;
                if (mediaFile.FileName.ToLower().Contains(".png"))
                {
                    imageformat = "image/png";
                }
                if (mediaFile.FileName.ToLower().Contains(".jpg"))
                {
                    imageformat = "image/jpg";
                }
                byte[] imageByte;
                var newFile = Path.Combine(FileSystem.CacheDirectory, mediaFile.FileName);
                var stream = await mediaFile.OpenReadAsync();
                using (MemoryStream memory = new MemoryStream())
                {
                    stream.CopyTo(memory);
                    imageByte = memory.ToArray();
                }

                //converting to base64string
                var convertedImage = Convert.ToBase64String(imageByte);
                var img = string.Format($"data:{imageformat};base64" + convertedImage);
                ImagestringUser = convertedImage;

                //converting from base64string to image
                var imgt = Convert.FromBase64String(convertedImage);
                MemoryStream stream2 = new(imgt);
                ImageSource image = ImageSource.FromStream(() => stream2);
                ImageUser = image;
            }
        }

        private async void executeTakePicCMD()
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {

                await Application.Current.MainPage.DisplayAlert("Oops!", "Camera không được hỗ trợ!", "OK");
                return;
            }

            try
            {
                FileResult mediafile = await MediaPicker.Default.CapturePhotoAsync();
                if (mediafile != null)
                {
                    byte[] imageByte;
                    var newFile = Path.Combine(FileSystem.CacheDirectory, mediafile.FileName);
                    var stream = await mediafile.OpenReadAsync();
                    using (MemoryStream memory = new MemoryStream())
                    {
                        stream.CopyTo(memory);
                        imageByte = memory.ToArray();
                    }

                    //converting to base64string
                    var convertedImage = Convert.ToBase64String(imageByte);
                    var img = string.Format("data:image/png;base64" + convertedImage);
                    ImagestringUser = convertedImage;
                    //converting from base64string to image
                    var imgt = Convert.FromBase64String(convertedImage);
                    MemoryStream stream2 = new(imgt);
                    ImageSource image = ImageSource.FromStream(() => stream2);
                    ImageUser = image;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Oops!", "Error!", "OK");
            }
        }

        private async void executeBackPageCMD()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void executeSettingsCMD()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SettingView());
        }

        private async void executeLogoutCMD()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Chờ đã", "Bạn thực sự muốn đăng xuất khỏi hệ thống chứ?", "Có", "Không");
            if (result)
            {
                dataLogin.Instance.currUser = new User();
                Application.Current.MainPage = new NavigationPage(new LoginView());
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
        void checkOldPass(string pass)
        {
            if(pass==string.Empty)
            {
                ErroroldPass = "Vui lòng nhập mật khẩu cũ!";
            }
            else if(GetMD5Hash(pass) != CurentUser.Password)
            {
                ErroroldPass = "Mật khẩu cũ không đúng!";
            }
            else
            {
                ErroroldPass = string.Empty;
            }
        }
        public void loadImage()
        {
            if (curentUser.ImageData != null)
            {
                ImagestringUser = curentUser.ImageData;
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(curentUser.ImageData));
                ImageSource image = ImageSource.FromStream(() => stream);
                ImageUser = image;
            }
            else
            {
                ImagestringUser = null;
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "noimage.png");
                ImageUser = ImageSource.FromFile(imagePath);
            }
        }
        void checkNewPass(string pass)
        {
            if (pass == string.Empty)
            {
                ErrornewPass = "Không bỏ trống mật khẩu!";
            }
            else if (pass.Length<6)
            {
                ErrornewPass = "Mật khẩu quá ngắn!";
            }
            else if(checkInvalidPass(pass))
            {
                ErrornewPass = "Mật khẩu không hợp lệ!";
            }
            else
            {
                ErrornewPass = string.Empty;
            }
        }
        void checkReNewPass(string pass)
        {
            if (pass != NewPass)
            {
                ErrorRePass = "Mật khẩu nhập lại không trùng khớp!";
            }
            else
            {
                ErrorRePass = string.Empty;
            }
        }
        private bool checkInvalidPass(string p)
        {
            bool hasChar = false;
            bool hasNum = false;
            foreach (var item in p)
            {
                if (Char.IsDigit(item))
                {
                    hasNum = true;
                }
                else
                {
                    hasChar = true;
                }
            }
            if(hasChar && hasNum)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ImageSource ImageUser { get => _imageUser; set => SetProperty(ref _imageUser, value); }
        public User CurentUser { get => curentUser; 
            set 
            { 
                SetProperty(ref curentUser, value);
                loadImage();
            } 
        }

        public string NameUser { get => nameUser; set => SetProperty(ref nameUser, value); }
        public string GenderUser { get => genderUser; set => SetProperty(ref genderUser, value); }
        public DateTime DatebirthUser { get => datebirthUser; set => SetProperty(ref datebirthUser, value); }
        public string CityUser { get => cityUser; set => SetProperty(ref cityUser, value); }
        public string DistrictUser { get => districtUser; set => SetProperty(ref districtUser, value); }
        public List<Province> ListCity { get => listCity; set => SetProperty(ref listCity, value); }
        public List<string> ListDistrict { get => listDistrict; set => SetProperty(ref listDistrict, value); }
        public int CitySelect { get => citySelect; 
            set 
            { 
                SetProperty(ref citySelect, value);
                CityUser = ListCity[value].province_name;
            } 
        }

        public string? ImagestringUser { get => _imagestringUser; set => SetProperty(ref _imagestringUser, value); }
        public string TextError { get => textError; set => SetProperty(ref textError, value); }
        public string OldPass { get => oldPass; 
            set 
            { 
                SetProperty(ref oldPass, value);
                checkOldPass(value);
            } 
        }
        public string NewPass { get => newPass; set 
            { 
                SetProperty(ref newPass, value); 
                checkNewPass(value);
            } 
        }
        public string RePass { get => rePass; set 
            { 
                SetProperty(ref rePass, value); 
                checkReNewPass(value);
            } 
        }
        public string ErroroldPass { get => erroroldPass; set => SetProperty(ref erroroldPass, value); }
        public string ErrornewPass { get => errornewPass; set => SetProperty(ref errornewPass, value); }
        public string ErrorRePass { get => errorRePass; set => SetProperty(ref errorRePass, value); }
    }
}
