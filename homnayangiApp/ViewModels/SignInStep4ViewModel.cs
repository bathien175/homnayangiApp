using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using SkiaSharp;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace homnayangiApp.ViewModels
{
    public class SignInStep4ViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private bool isLoading = false;
        private List<tagControl> listTag = new List<tagControl>();
        private string textFilter = string.Empty;
        private List<tagControl> listTagFilter = new List<tagControl>();
        public List<tagControl> ListTag { get => listTag;
            set
            {
                SetProperty(ref listTag, value);
                FilterResult();
            }
        }
        public DelegateCommand GoStep5Cmd { get; }
        public DelegateCommand BackStepCmd { get; }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public string TextFilter { get => textFilter; 
            set 
            { 
                SetProperty(ref textFilter, value);
                FilterResult();
            } 
        }
        public List<tagControl> ListTagFilter { get => listTagFilter; set => SetProperty(ref listTagFilter, value); }

        public SignInStep4ViewModel()
        {
            _userService = new UserService();
            loadTag();
            checkTag();
            GoStep5Cmd = new DelegateCommand(executeGoStep5CMD);
            BackStepCmd = new DelegateCommand(executeBackStepCMD);
        }
        private async void executeGoStep5CMD()
        {
            //Điền tag
            if (ListTag.Where(x => x.IsChecked == true).Count() <= 5 && ListTag.Where(x => x.IsChecked == true).Count() > 0)
            {
                var a = await Shell.Current.DisplayAlert("Xác nhận", "Bạn có chắc chắn với các thông tin tài khoản chứ?", "Có", "Không");
                if (a)
                {
                    IsLoading = true;
                    User u = new User();
                    u.IDUser = dataSignIn.Instance.userID;
                    u.Phone = dataSignIn.Instance.userPhone;
                    u.Name = dataSignIn.Instance.userName;
                    u.Gender = dataSignIn.Instance.userGender;
                    u.DateBirth = dataSignIn.Instance.userDatebirth;
                    if (dataSignIn.Instance.userImageByte != null)
                    {
                        u.ImageData = dataSignIn.Instance.userImageByte;
                    }
                    u.City = dataSignIn.Instance.userCity;
                    u.Dictrict = dataSignIn.Instance.userDistrict;
                    u.Password = GetMD5Hash(dataSignIn.Instance.userPass);
                    u.Tags = ListTag.Where(x => x.IsChecked == true).Select(ta => ta.TagName).ToList();

                    try
                    {
                        await _userService.Create(u);
                        dataSignIn.Instance.clearData();
                        await Shell.Current.GoToAsync("//SignInStep5");
                        IsLoading = false;
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Lỗi", "Server đã xảy ra lỗi phản hồi", "Thử lại");
                        IsLoading = false;
                    }
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Lỗi", "Chọn ít nhất 1 TAG và tối đa 5 TAG", "Đã hiểu");
            }
        }

        private void FilterResult()
        {
            //lọc kết quả các tag
            if (TextFilter == string.Empty)
                ListTagFilter = ListTag;
            else
                ListTagFilter = new List<tagControl>(ListTag.Where(x => x.TagName.ToLower().Contains(TextFilter.ToLower())).ToList());
        }
        private async void executeBackStepCMD()
        {
            List<tagControl> listnew = ListTag.Where(x => x.IsChecked == true).ToList();
            dataSignIn.Instance.listTag = listnew;
            await Shell.Current.GoToAsync("//SignInStep3");
        }
        private void checkTag()
        {
            var a = dataSignIn.Instance.listTag;
            if (a.Count > 0)
            {
                foreach (var tag in a)
                {
                    var find = ListTag.Where(x => x.TagName == tag.TagName).FirstOrDefault();
                    if (find != null)
                    {
                        find.IsChecked = true;
                    }
                }
            }
        }
        #region loaddata
        private async void loadTag()
        {
            IsLoading = true;
            await Task.Run(async () =>
            {
                List<tagControl> listnew = [];
                ITagsService _tags = new TagsService();
                var a = await _tags.Get();
                if (a.Count > 0)
                {
                    foreach (var item in a)
                    {
                        listnew.Add(new tagControl() { TagName = item.Name });
                    }
                }
                ListTag = listnew.OrderBy(x => x.TagName).ToList();
                IsLoading = false;
            });
        }
        #endregion
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
    }
}
