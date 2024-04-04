using homnayangiApp.Commands;
using homnayangiApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.Models
{
    public class FindUserModel : BaseViewModel
    {
        private ImageSource? imgUser;
        private string nameUser = string.Empty;
        private string phoneRealUser = string.Empty;
        private string phoneFakeUser = string.Empty;
        private string imageString = string.Empty;
        private DelegateCommand<string> restorePassword;
        public ImageSource? ImgUser { get => imgUser; set => SetProperty(ref imgUser, value); }
        public string NameUser { get => nameUser; set => SetProperty(ref nameUser, value); }
        public string PhoneRealUser { get => phoneRealUser; set => SetProperty(ref phoneRealUser, value); }
        public string PhoneFakeUser { get => phoneFakeUser; set => SetProperty(ref phoneFakeUser, value); }
        public string ImageString { get => imageString; set => SetProperty(ref imageString, value); }
        public DelegateCommand<string> RestorePassword { get => restorePassword; set => SetProperty(ref restorePassword, value); }

        public FindUserModel()
        {

        }
    }
}
