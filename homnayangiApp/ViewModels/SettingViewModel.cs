using homnayangiApp.Commands;
using homnayangiApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public DelegateCommand backPageCMD { get; }

        public SettingViewModel()
        {
            backPageCMD = new DelegateCommand(executeBackPageCMD);
        }

        private async void executeBackPageCMD()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}
