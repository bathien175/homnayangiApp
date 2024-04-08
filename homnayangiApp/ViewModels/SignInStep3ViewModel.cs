using homnayangiApp.Commands;
using homnayangiApp.CustomControls;
using homnayangiApp.Models;
using homnayangiApp.ViewModels.Base;
using homnayangiApp.Views;
using System.Globalization;
using System.Text.Json;

namespace homnayangiApp.ViewModels
{
    public class SignInStep3ViewModel : BaseViewModel
    {
        private List<Province> listCity = dataCity.Instance.listProvince;
        private List<String> listDistrict = dataCity.Instance.listDistrict;
        private int city = dataSignIn.Instance.userCityId;
        private string districtSelect = string.Empty;
        private string provinceSelect = string.Empty;
        private string genderSelect = dataSignIn.Instance.userGender;
        private DateTime datebirth = DateTime.Today.Date;
        private bool isLoading = false;


        public List<Province> ListCity { get => listCity; set => SetProperty(ref listCity, value); }
        public List<string> ListDistrict { get => listDistrict; set => SetProperty(ref listDistrict, value); }
        public int City
        {
            get => city; set
            {
                SetProperty(ref city, value);
                ProvinceSelect = ListCity[value].province_name;
            }
        }
        public string DistrictSelect { get => districtSelect; set => SetProperty(ref districtSelect, value); }
        public string ProvinceSelect { get => provinceSelect; set => SetProperty(ref provinceSelect, value); }
        public List<string> listGender { get; set; } = new List<string>() { "Nam", "Nữ", "Khác" };
        public string GenderSelect { get => genderSelect; set => SetProperty(ref genderSelect, value); }
        public DateTime Datebirth { get => datebirth; set => SetProperty(ref datebirth, value); }

        public DelegateCommand GoStep4Cmd { get; }
        public DelegateCommand BackStepCmd { get; }
        public DelegateCommand selectCityCmd { get; }
        public DelegateCommand selectDistricCmd { get; }
        public DelegateCommand appearView { get; }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public SignInStep3ViewModel()
        {
            GenderSelect = dataSignIn.Instance.userGender;
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime result = DateTime.ParseExact(dataSignIn.Instance.userDatebirth, "dd-MM-yyyy", provider);
            Datebirth = result;
            BackStepCmd = new DelegateCommand(executeBackStepCMD);
            GoStep4Cmd = new DelegateCommand(executeGoStep4CMD);
            if(dataSignIn.Instance.userDistrict == string.Empty)
            {
                //chưa set dữ liệu
                DistrictSelect = ListDistrict[0];
            }
            else
            {
                //đã set dữ liệu
                City = dataSignIn.Instance.userCityId;
                DistrictSelect = dataSignIn.Instance.userDistrict;
            }
        }

        private async void executeGoStep4CMD()
        {
            //Điền thông tin cá nhân
            IsLoading = true;
            saveData();
            IsLoading = false;
            await Shell.Current.GoToAsync("//SignInStep4");
        }
        private async void executeBackStepCMD()
        {
            saveData();
            await Shell.Current.GoToAsync("//SignInStep3");
        }
        void saveData()
        {
            dataSignIn.Instance.userGender = GenderSelect;
            dataSignIn.Instance.userCityId = City;
            dataSignIn.Instance.userDistrict = DistrictSelect;
            dataSignIn.Instance.userCity = ProvinceSelect;
            dataSignIn.Instance.userDatebirth = Datebirth.ToString("dd-MM-yyyy");
        }
    }
}
