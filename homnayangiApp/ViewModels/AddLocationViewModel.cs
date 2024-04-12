using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using System.Collections.ObjectModel;

namespace homnayangiApp.ViewModels
{
    public class AddLocationViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private List<string> listImageString = new List<string>();
        private ObservableCollection<ImageSource> listImageSource = new ObservableCollection<ImageSource>();
        private string citySelect = string.Empty;
        private string districtSelect = string.Empty;
        private List<Province> listCity = dataCity.Instance.listProvince;
        private List<String> listDistrict = dataCity.Instance.listDistrict;
        private int cityIndexSelect;
        private List<string> listTag = new List<string>();
        private ObservableCollection<string> tagSelect = new ObservableCollection<string>();
        private long minPrice;
        private long maxPrice;
        private TimeSpan openTime = new TimeSpan(9,0,0);
        private TimeSpan closeTime = new TimeSpan(10, 0, 0);
        private bool isOpen24H = false;
        private string phone = string.Empty;
        private string address = string.Empty;
        private bool isEmpty = false;
        private string nameLocation = string.Empty;
        private bool isLoading = false;

        public List<string> ListImageString { get => listImageString; 
            set 
            { 
                SetProperty(ref listImageString, value);
                if (value.Count == 0)
                {
                    IsEmpty = true;
                    ListImageSource.Clear();
                }
                else
                {
                    IsEmpty = false;
                    ObservableCollection < ImageSource > listnew = new ObservableCollection < ImageSource >();
                    foreach (var item in value)
                    {
                        //converting from base64string to image
                        var imgt = Convert.FromBase64String(item);
                        MemoryStream stream2 = new(imgt);
                        ImageSource image = ImageSource.FromStream(() => stream2);
                        listnew.Add(image);
                    }
                    ListImageSource = listnew;
                }
            } 
        }
        public ObservableCollection<ImageSource> ListImageSource { get => listImageSource;
            set
            {
                SetProperty(ref listImageSource, value);
            }
        }
        public string CitySelect { get => citySelect; set => SetProperty(ref citySelect, value); }
        public string DistrictSelect { get => districtSelect; set => SetProperty(ref districtSelect, value); }
        public List<Province> ListCity { get => listCity; set => SetProperty(ref listCity, value); }
        public List<string> ListDistrict { get => listDistrict; set => SetProperty(ref listDistrict, value); }
        public int CityIndexSelect { get => cityIndexSelect; set 
            { 
                SetProperty(ref cityIndexSelect, value);
                CitySelect = ListCity[value].province_name;
            } 
        }
        public List<string> ListTag { get => listTag; set => SetProperty(ref listTag, value); }
        public ObservableCollection<string> TagSelect { get => tagSelect;
            set
            {
                SetProperty(ref tagSelect, value);
            }
        }
        public long MinPrice { get => minPrice; set => SetProperty(ref minPrice, value); }
        public long MaxPrice { get => maxPrice; set => SetProperty(ref maxPrice, value); }
        public TimeSpan OpenTime { get => openTime; set => SetProperty(ref openTime, value); }
        public TimeSpan CloseTime { get => closeTime; set => SetProperty(ref closeTime, value); }
        public bool IsOpen24H { get => isOpen24H; set => SetProperty(ref isOpen24H, value); }
        public string Phone { get => phone; set => SetProperty(ref phone, value); }
        public string Address { get => address; set => SetProperty(ref address, value); }
        public bool IsEmpty { get => isEmpty; set => SetProperty(ref isEmpty, value); }

        public AddLocationViewModel()
        {
            _locationService = new LocationService();
            loadTag();
            ListImageString = new List<string>();
            ChooseImageCMD = new DelegateCommand(executeChooseImageCMD);
            CreateLocationCMD = new DelegateCommand(executeCreateLocationCMD);
            removeImage = new DelegateCommand<string>(executeRemoveImageCMD);
        }

        private void executeRemoveImageCMD(string s)
        {
            List<string> list = new List<string>();
            foreach (var item in ListImageString)
            {
                if(item != s)
                {
                    list.Add(item);
                }
            }
            ListImageString = list;
        }

        private async void executeChooseImageCMD()
        {
            var pickImages = await FilePicker.PickMultipleAsync(new PickOptions()
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Chọn tối đa 10 ảnh"
            });

            if(pickImages!= null)
            {
                if(pickImages.Count() > 10)
                {
                    await Application.Current.MainPage.DisplayAlert("Xin lỗi", "Chỉ được chọn tối đa 10 ảnh", "Đã hiểu");
                }
                else
                {
                    List<string> imagestringnew = new List<string>();
                    foreach (var item in pickImages)
                    {
                        byte[] imageByte;
                        var newFile = Path.Combine(FileSystem.CacheDirectory, item.FileName);
                        var stream = await item.OpenReadAsync();
                        using (MemoryStream memory = new MemoryStream())
                        {
                            stream.CopyTo(memory);
                            imageByte = memory.ToArray();
                        }
                        //converting to base64string
                        var convertedImage = Convert.ToBase64String(imageByte);
                        imagestringnew.Add(convertedImage);
                    }
                    ListImageString = imagestringnew;
                }
            }
        }

        private async void executeCreateLocationCMD()
        {
            if (CloseTime <= OpenTime && IsOpen24H == false)
            {
                await Application.Current.MainPage.DisplayAlert("Thất bại!","Thời gian hoạt động không hợp lệ! Vui lòng kiểm tra và thử lại","OK");
            }else if(MaxPrice < MinPrice)
            {
                await Application.Current.MainPage.DisplayAlert("Thất bại!", "Giá bán không hợp lệ! Vui lòng kiểm tra và thử lại", "OK");
            }
            else if(TagSelect.Count ==0)
            {
                await Application.Current.MainPage.DisplayAlert("Thất bại!", "Vui lòng chọn Tags, tối đa 10 tags", "OK");
            }else if (TagSelect.Count >10)
            {
                await Application.Current.MainPage.DisplayAlert("Thất bại!", "Chỉ được chọn tối đa 10 Tags!", "OK");
            }else if(ListImageString.Count() == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Thất bại!", "Hãy chọn ít nhất 1 tấm ảnh", "OK");
            } 
            else
            {
                IsLoading = true;
                try
                {
                    Models.Location l = new Models.Location
                    {
                        Name = NameLocation,
                        HotLine = Phone,
                        Address = Address,
                        Province = CitySelect,
                        District = DistrictSelect
                    };
                    if (IsOpen24H)
                    {
                        l.IsOpen24H = true;
                        l.OpenTime = null;
                        l.CloseTime = null;
                    }
                    else
                    {
                        l.IsOpen24H = false;
                        l.OpenTime = OpenTime;
                        l.CloseTime = CloseTime;
                    }
                    l.MinPrice = MinPrice;
                    l.MaxPrice = MaxPrice;
                    if (dataLogin.Instance.IsAdmin)
                    {
                        l.Creator = null;
                    }
                    else
                    {
                        l.Creator = dataLogin.Instance.currUser.Id;
                    }
                    if (ListImageString.Count > 0)
                    {
                        l.Images = ListImageString;
                    }
                    else
                    {
                        l.Images = null;
                    }
                    l.Tags = new List<String>(TagSelect);
                    l.IsShare = true;
                    await _locationService.Create(l);
                    await Application.Current.MainPage.DisplayAlert("Thành công!", "Tạo địa điểm mới thành công!", "OK");
                    IsLoading = false;
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Thất bại!", "Server xảy ra lỗi! Không thể ghi dữ liệu!", "Thử lại");
                    IsLoading = false;
                }
            }
        }

        public DelegateCommand CreateLocationCMD { get; }
        public DelegateCommand ChooseImageCMD { get; }
        public DelegateCommand<string> removeImage { get; }
        public string NameLocation { get => nameLocation; set => SetProperty(ref nameLocation, value) ; }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        private async void loadTag()
        {
            IsLoading = true;
            await Task.Run(async () =>
            {
                ITagsService _tags = new TagsService();
                var a = await _tags.Get();
                List<string> listnew = [];
                if (a.Count > 0)
                {
                    foreach (var item in a)
                    {
                        listnew.Add(item.Name);
                    }
                }
                ListTag = listnew.OrderBy(s => s).ToList();
                IsLoading = false;
            });
        }
    }
}
