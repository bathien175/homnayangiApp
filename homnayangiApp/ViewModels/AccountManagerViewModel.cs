using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace homnayangiApp.ViewModels
{
    public class AccountManagerViewModel : BaseViewModel
    {
        private string textError = string.Empty;
        private readonly IUserService _userService;
        private ImageSource? _imagestringUser;
        private string imagestr = string.Empty;
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
        private bool isLoading = false;
        private List<String> listGender = new List<string>();
        private bool isExecuteCMD = false;

        public List<string> ListTag { get => listTag; set => SetProperty(ref listTag, value); }
        public ObservableCollection<string> TagSelect { get => tagSelect;
            set 
            { 
                SetProperty(ref tagSelect, value);
            }
        }
        public DelegateCommand logOutCMD { get; }
        public DelegateCommand gotoSettingCMD { get; }
        public DelegateCommand gotoInformationCMD { get; }
        public DelegateCommand gotoChangePasswordCMD { get; }
        public DelegateCommand BackPageCMD { get; }
        public DelegateCommand InfoBackPageCMD { get; }
        public DelegateCommand TakePic { get; }
        public DelegateCommand ChoosePic { get; }
        public DelegateCommand ResetPic { get; }
        public DelegateCommand UpdateInformation { get; }
        public DelegateCommand ChangePassCMD { get; }
        public AccountManagerViewModel()
        {
            _userService = new UserService();
            loadDta();
            logOutCMD = new DelegateCommand(executeLogoutCMD);
            gotoSettingCMD = new DelegateCommand(executeSettingsCMD);
            BackPageCMD = new DelegateCommand(executeBackPageCMD);
            InfoBackPageCMD = new DelegateCommand(executeInfoBackPageCMD);
            TakePic = new DelegateCommand(executeTakePicCMD);
            ChoosePic = new DelegateCommand(executeChoosePicCMD);
            ResetPic = new DelegateCommand(executeResetPicCMD);
            UpdateInformation = new DelegateCommand(executeUpdateInfoCMD);
            gotoChangePasswordCMD = new DelegateCommand(executeGotoChangePassCMD);
            ChangePassCMD = new DelegateCommand(executeChangePassCMD);
            gotoInformationCMD = new DelegateCommand(executegotoInformationCMD);
        }


        private async void loadDta()
        {
            IsLoading = true;
            await Task.Run(() =>
            {
                CurentUser = dataLogin.Instance.currUser;
                loadTag();
                TagSelect = new ObservableCollection<string>(CurentUser.Tags);
                ListGender = new List<string> { "Nam", "Nữ", "Khác" };
                NameUser = CurentUser.Name;
                GenderUser = CurentUser.Gender;
                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime result = DateTime.ParseExact(CurentUser.DateBirth, "dd-MM-yyyy", provider);
                DatebirthUser = result;
                CitySelect = ListCity.IndexOf(ListCity.Where(x => x.province_name == CurentUser.City).First());
                DistrictUser = CurentUser.Dictrict;
                IsLoading = false;
            });
        }
        private async void executegotoInformationCMD()
        {
            if (IsExecuteCMD == true)
                return;
            IsLoading = true;
            IsExecuteCMD = true;
            var v = await Task.Run(() => new AccountManagerView());
            await Shell.Current.Navigation.PushModalAsync(v);
            IsLoading = false;
            IsExecuteCMD = false;
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
                await _userService.Update(u.Id,u);
                dataLogin.Instance.currUser = u;
                CurentUser = u;
                await Shell.Current.DisplayAlert("Thành công","Đổi mật khẩu thành công","OK");
                await Shell.Current.Navigation.PopAsync(true);
            }
        }

        private async void executeGotoChangePassCMD()
        {
            await Shell.Current.Navigation.PushAsync(new ChangePasswordView());
        }

        private async void loadTag()
        {
            IsLoading = true;
            await Task.Run(async () =>
            {
                ListTag.Clear();
                ITagsService _tags = new TagsService();
                var a = await _tags.Get();
                if (a.Count > 0)
                {
                    foreach (var item in a)
                    {
                        ListTag.Add(item.Name);
                    }
                }
                IsLoading = false;
            });
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
                IsLoading = true;
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
                if (!String.IsNullOrEmpty(Imagestr))
                {
                    var imgt = Convert.FromBase64String(Imagestr);
                    MemoryStream stream2 = new(imgt);
                    string im = await _userService.UploadUserImage(u.Id, stream2);
                    u.ImageData = im;
                }
                else
                {
                    u.ImageData = dataLogin.Instance.currUser.ImageData;
                }
                try
                {
                    await _userService.Update(u.Id, u);
                    dataLogin.Instance.currUser = u;
                    CurentUser = u;
                    IsLoading = false;
                    await Shell.Current.DisplayAlert("Thành công","Cập nhật thông tin cá nhân thành công","OK");
                }
                catch (Exception)
                {
                    IsLoading = false;
                    await Shell.Current.DisplayAlert("Thất bại", "Server xảy ra lỗi trong quá trình đọc dữ liệu", "Thử lại");
                }
            }
        }

        private void executeResetPicCMD()
        {
            loadImage();
            Imagestr = string.Empty;
        }

        private async void executeChoosePicCMD()
        {
            var mediaFile = await MediaPicker.PickPhotoAsync();
            if (mediaFile != null)
            {
                string imageformat = string.Empty;
                byte[] imageByte;
                var stream = await mediaFile.OpenReadAsync();
                using (MemoryStream memory = new MemoryStream())
                {
                    stream.CopyTo(memory);
                    imageByte = memory.ToArray();
                }
                var convertedImage = Convert.ToBase64String(imageByte);
                Imagestr = convertedImage;

                //converting to base64string
                var imgt = Convert.FromBase64String(convertedImage);
                MemoryStream stream2 = new(imgt);
                ImageSource image = ImageSource.FromStream(() => stream2);
                ImagestringUser = image;

            }
        }

        private async void executeTakePicCMD()
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {

                await Shell.Current.DisplayAlert("Oops!", "Camera không được hỗ trợ!", "OK");
                return;
            }

            try
            {
                var mediafile = await MediaPicker.Default.CapturePhotoAsync();
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
                    var convertedImage = Convert.ToBase64String(imageByte);
                    Imagestr = convertedImage;

                    //converting to base64string
                    var imgt = Convert.FromBase64String(convertedImage);
                    MemoryStream stream2 = new(imgt);
                    ImageSource image = ImageSource.FromStream(() => stream2);
                    ImagestringUser = image;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Oops!", "Error!", "OK");
            }
        }

        private async void executeBackPageCMD()
        {
            await Shell.Current.Navigation.PopAsync(true);
        }
        private async void executeInfoBackPageCMD()
        {
            await Shell.Current.GoToAsync("//HomeApp//Personal");
        }
        private async void executeSettingsCMD()
        {
            if (IsExecuteCMD == true)
                return;
            IsExecuteCMD = true;
            IsLoading = true;
            var v = await Task.Run(() => new SettingView());
            await Shell.Current.Navigation.PushModalAsync(v);
            IsLoading = false; 
            IsExecuteCMD = false;
        }

        private async void executeLogoutCMD()
        {
            var result = await Shell.Current.DisplayAlert("Chờ đã", "Bạn thực sự muốn đăng xuất khỏi hệ thống chứ?", "Có", "Không");
            if (result)
            {
                dataLogin.Instance.currUser = new User();
                await Shell.Current.GoToAsync("//Login");
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
            if (CurentUser.ImageData != null)
            {
                ImagestringUser = CurentUser.ImageData;
            }
            else
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "noimage.png");
                ImagestringUser = ImageSource.FromFile(imagePath);
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

        public ImageSource? ImagestringUser { get => _imagestringUser; set => SetProperty(ref _imagestringUser, value); }
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
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public List<string> ListGender { get => listGender; set => SetProperty(ref listGender, value); }
        public bool IsExecuteCMD { get => isExecuteCMD; set => SetProperty(ref isExecuteCMD, value); }
        public string Imagestr { get => imagestr; set => SetProperty(ref imagestr, value); }
    }
}
