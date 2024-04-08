using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using Plugin.Media.Abstractions;
using SkiaSharp;
using System;
using static FFImageLoading.Work.ImageInformation;

namespace homnayangiApp.ViewModels
{
    public class SignInStep2ViewModel : BaseViewModel
    {
        private ImageSource imageSrc = string.Empty;
        private string idUser = string.Empty;
        private string byteImage = string.Empty;
        private string errorID = string.Empty;
        private bool isLoading = false;
        public ImageSource ImageSrc { get => imageSrc; set => SetProperty(ref imageSrc, value); }

        public DelegateCommand<string> ChangeImageCmd { get; }
        public DelegateCommand GoStep3Cmd { get; }
        public DelegateCommand BackStepCmd { get; }
        public string IDUser { get => idUser; 
            set 
            { 
                SetProperty(ref idUser, value);
                if (value.Length == 0)
                {
                    ErrorID = "Không bỏ trống ID người dùng!";
                }
                else
                {
                    if(value.Length < 6)
                    {
                        ErrorID = "ID người dùng quá ngắn!";
                    }
                    else
                    {
                        if (HasSpecialCharacters(value))
                        {
                            //có ký tự đặc biệt
                            ErrorID = "ID người dùng không được phép có ký tự đặc biệt!";
                        }
                        else
                        {
                            //không có ký tự đặc biệt
                            ErrorID = string.Empty;
                        }
                    }
                }
            } 
        }
        static bool HasSpecialCharacters(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
                {
                    return true;
                }
            }
            return false;
        }
        public string ByteImage { get => byteImage; set 
            { 
                SetProperty(ref byteImage, value); 
            } 
        }

        public string ErrorID { get => errorID; set => SetProperty(ref errorID, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public SignInStep2ViewModel()
        {
            if (dataSignIn.Instance.userImageByte == string.Empty || dataSignIn.Instance.userImageByte == null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "noimage.png");
                ImageSrc = ImageSource.FromFile(imagePath);
            }
            else
            {
                var imgt = Convert.FromBase64String(dataSignIn.Instance.userImageByte);
                MemoryStream stream2 = new(imgt);
                ImageSource image = ImageSource.FromStream(() => stream2);
                ImageSrc = image;
            }
            if (dataSignIn.Instance.userID != string.Empty)
            {
                IDUser = dataSignIn.Instance.userID;
            }
            GoStep3Cmd = new DelegateCommand(executeGoStep3CMD);
            BackStepCmd = new DelegateCommand(executeBackStepCMD);
            ChangeImageCmd = new DelegateCommand<string>(executeChangeImageCMD);
        }
        private async void executeChangeImageCMD(string vl)
        {
            int sl = Convert.ToInt32(vl);
            if (sl==0)
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
                    var stream = await mediaFile.OpenReadAsync();
                    using (MemoryStream memory = new MemoryStream())
                    {
                        stream.CopyTo(memory);
                        imageByte = memory.ToArray();
                    }

                    //converting to base64string
                    var convertedImage = Convert.ToBase64String(imageByte);
                    var img = string.Format($"data:{imageformat};base64," + convertedImage);
                    dataSignIn.Instance.userImageByte = convertedImage;

                    //converting from base64string to image
                    var imgt = Convert.FromBase64String(convertedImage);
                    MemoryStream stream2 = new(imgt);
                    ImageSource image = ImageSource.FromStream(() => stream2);
                    ImageSrc = image;
                }
            }
            else
            {
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    
                    await Shell.Current.DisplayAlert("Lỗi", "Máy không hỗ trợ camera", "OK");
                    return;
                }

                try
                {
                    var mediafile = await MediaPicker.Default.CapturePhotoAsync();
                    if (mediafile != null)
                    {
                        byte[] imageByte;
                        var stream = await mediafile.OpenReadAsync();
                        using (MemoryStream memory = new MemoryStream())
                        {
                            stream.CopyTo(memory);
                            imageByte = memory.ToArray();
                        }

                        //converting to base64string
                        var convertedImage = Convert.ToBase64String(imageByte);
                        var img = string.Format("data:img/png;base64,"+ convertedImage);
                        dataSignIn.Instance.userImageByte = convertedImage;
                        //converting from base64string to image
                        var imgt = Convert.FromBase64String(convertedImage);
                        MemoryStream stream2 = new(imgt);
                        ImageSource image = ImageSource.FromStream(() => stream2);
                        ImageSrc = image;
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Oops!", "Error!", "OK");
                }
            }
        }


        private async void executeGoStep3CMD()
        {
            IsLoading = true;
            //chọn ảnh
            if(ErrorID == string.Empty)
            {
                if (IDUser.Length == 0)
                {
                    ErrorID = "Không bỏ trống ID người dùng!";
                    IsLoading = false;
                }
                else
                {
                    IUserService u = new UserService();
                    var find = await u.Get(IDUser);
                    if (find != null)
                    {
                        ErrorID = "ID người dùng đã tồn tại! Vui lòng thử thay ID khác!";
                        IsLoading = false;
                    }
                    else
                    {
                        ErrorID = string.Empty;
                        dataSignIn.Instance.userID = IDUser;
                        IsLoading = false;
                        await Shell.Current.GoToAsync("//SignInStep3");
                    }
                }
            }
        }
        private async void executeBackStepCMD()
        {
            dataSignIn.Instance.userID = IDUser;
            await Shell.Current.GoToAsync("//SignIn");
        }
    }
}
