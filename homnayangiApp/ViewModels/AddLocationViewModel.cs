using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ModelService;
using homnayangiApp.ViewModels.Base;
using System.Collections.ObjectModel;
using static FFImageLoading.Work.ImageInformation;

namespace homnayangiApp.ViewModels
{
    public class AddLocationViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private ObservableCollection<string> listImageString = new ObservableCollection<string>();
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
        private bool isEmpty = true;
        private string nameLocation = string.Empty;
        private bool isLoading = false;
        private string locateIdCurrent = string.Empty;
        private string locateIdCache = string.Empty;
        private bool isAddNew = true;
        private bool isShare = true;
        private string creator = string.Empty;
        private bool isExecuteCMD = false;

        public DelegateCommand CreateLocationCMD { get; }
        public DelegateCommand ChooseImageCMD { get; }
        public DelegateCommand AddmageCMD { get; }
        public DelegateCommand backPage { get; }
        public DelegateCommand<String> removeImage { get; }

        public ObservableCollection<string> ListImageString { get => listImageString; 
            set 
            { 
                SetProperty(ref listImageString, value);
                if (value.Count == 0)
                {
                    IsEmpty = true;
                }
                else
                {
                    IsEmpty = false;
                }
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
        public string NameLocation { get => nameLocation; set => SetProperty(ref nameLocation, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public string LocateIdCurrent { get => locateIdCurrent; 
            set 
            { 
                SetProperty(ref locateIdCurrent, value); 
                if(value != string.Empty)
                {
                    IsAddNew = false;
                    loadDataLocation(value);
                }
                else
                {
                    IsAddNew = true;
                    IsEmpty = true;
                    //LocateIdCache = GenerateRandomString(20);
                }
            } 
        }

        public bool IsAddNew { get => isAddNew; set => SetProperty(ref isAddNew, value); }
        public bool IsShare { get => isShare; set => SetProperty(ref isShare, value); }
        public string Creator { get => creator; set => SetProperty(ref creator, value); }
        public string LocateIdCache { get => locateIdCache; set => SetProperty(ref locateIdCache, value); }
        public bool IsExecuteCMD { get => isExecuteCMD; set => SetProperty(ref isExecuteCMD, value); }

        public AddLocationViewModel()
        {
            _locationService = new LocationService();
            loadTag();
            CityIndexSelect = 0;
            ListImageString = new ObservableCollection<string>();
            ChooseImageCMD = new DelegateCommand(executeChooseImageCMD);
            AddmageCMD = new DelegateCommand(executeAddImageCMD);
            CreateLocationCMD = new DelegateCommand(executeCreateLocationCMD);
            removeImage = new DelegateCommand<String>(executeRemoveImageCMD);
            backPage = new DelegateCommand(executebackPageCMD);
        }

        private async void executeAddImageCMD()
        {
            var pickImages = await FilePicker.PickMultipleAsync(new PickOptions()
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Chọn tối đa 10 ảnh"
            });

            if (pickImages != null)
            {
                if (pickImages.Count() + ListImageString.Count() > 10)
                {
                    await Shell.Current.DisplayAlert("Xin lỗi", "Chỉ được chọn tối đa 10 ảnh", "Đã hiểu");
                }
                else
                {
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
                        ListImageString.Add(convertedImage);
                    }
                }
            }
        }

        private async void executebackPageCMD()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public void executeRemoveImageCMD(String img)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            foreach (var item in ListImageString)
            {
                if(item != img)
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
                if(pickImages.Count() + ListImageString.Count() > 10)
                {
                    await Shell.Current.DisplayAlert("Xin lỗi", "Chỉ được chọn tối đa 10 ảnh", "Đã hiểu");
                }
                else
                {
                    ObservableCollection<string> imagestringnew = new ObservableCollection<string>();
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
        private async void loadDataLocation(string id)
        {
            var s = await _locationService.Get(id);
            NameLocation = s.Name;
            Phone = s.HotLine;
            IsOpen24H = s.IsOpen24H;
            if (!IsOpen24H)
            {
                OpenTime = s.OpenTime.Value;
                CloseTime = s.CloseTime.Value;
            }
            Address = s.Address;
            var pselect = ListCity.Where(x => x.province_name == s.Province).First();
            CityIndexSelect = ListCity.IndexOf(pselect);
            DistrictSelect = s.District;
            MaxPrice = s.MaxPrice;
            MinPrice = s.MinPrice;
            Creator = s.Creator;
            IsShare = s.IsShare;
            if (s.Images == null)
            {
                IsEmpty = true;
            }
            else
            {
                IsEmpty = false;
                var listn = await loadImageOld(s.Images);
                ListImageString = new ObservableCollection<string>(listn);
            }
            TagSelect = new ObservableCollection<string>(s.Tags);
        }
        private async Task<List<string>> loadImageOld(List<string>? images)
        {
            List<string> listnew = [];
            if (images!= null)
            {
                foreach (var image in images)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        // Tải hình ảnh từ URL
                        byte[] imageData = await httpClient.GetByteArrayAsync(image);
                        // Tạo một MemoryStream từ dữ liệu hình ảnh
                        var convertedImage = Convert.ToBase64String(imageData);
                        listnew.Add(convertedImage);
                    }
                }
            }
            return listnew;
        }
        private async void executeCreateLocationCMD()
        {
            if(IsExecuteCMD)
            { return; }

            IsExecuteCMD = true;
            if (CloseTime <= OpenTime && IsOpen24H == false)
            {
                await Shell.Current.DisplayAlert("Thất bại!","Thời gian hoạt động không hợp lệ! Vui lòng kiểm tra và thử lại","OK");
            }else if(MaxPrice < MinPrice)
            {
                await Shell.Current.DisplayAlert("Thất bại!", "Giá bán không hợp lệ! Vui lòng kiểm tra và thử lại", "OK");
            }
            else if(TagSelect.Count ==0)
            {
                await Shell.Current.DisplayAlert("Thất bại!", "Vui lòng chọn Tags, tối đa 10 tags", "OK");
            }else if (TagSelect.Count >10)
            {
                await Shell.Current.DisplayAlert("Thất bại!", "Chỉ được chọn tối đa 10 Tags!", "OK");
            }else if(ListImageString.Count() == 0)
            {
                await Shell.Current.DisplayAlert("Thất bại!", "Hãy chọn ít nhất 1 tấm ảnh", "OK");
            } 
            else
            {
                try
                {
                    if(IsAddNew == true)
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
                            l.Images = new List<string>(ListImageString);
                        }
                        else
                        {
                            l.Images = null;
                        }
                        l.Tags = new List<String>(TagSelect);
                        l.IsShare = IsShare;
                        await _locationService.Create(l);
                        await Shell.Current.DisplayAlert("Thành công!", "Tạo địa điểm mới thành công!", "OK");
                        await Shell.Current.Navigation.PopAsync();
                    }
                    else
                    {
                        var find = await _locationService.Get(LocateIdCurrent);
                        find.Name = NameLocation;
                        find.MaxPrice = MaxPrice;
                        find.MinPrice = MinPrice;
                        if (IsOpen24H)
                        {
                            find.IsOpen24H = true;
                            find.OpenTime = null;
                            find.CloseTime = null;
                        }
                        else
                        {
                            find.IsOpen24H = false;
                            find.OpenTime = OpenTime;
                            find.CloseTime = CloseTime;
                        }
                        find.District = DistrictSelect;
                        find.Province = CitySelect;
                        find.Address = Address;
                        find.HotLine = Phone;
                        if (ListImageString.Count > 0)
                        {
                            find.Images = new List<string>( ListImageString);
                        }
                        else
                        {
                            find.Images = null;
                        }
                        find.Tags = new List<String>(TagSelect);
                        find.IsShare = IsShare;
                        await _locationService.Update(find.Id,find);
                        await Shell.Current.DisplayAlert("Thành công!", "Sửa địa điểm thành công!", "OK");
                        await Shell.Current.Navigation.PopAsync();
                    }
                }
                catch (Exception)
                {
                    await Shell.Current.DisplayAlert("Thất bại!", "Server xảy ra lỗi! Không thể ghi dữ liệu!", "Thử lại");
                }
            }
            IsExecuteCMD = false;
        }

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
