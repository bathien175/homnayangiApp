
using homnayangiApp.Commands;
using homnayangiApp.ViewModels.Base;
using UraniumUI.Icons.FontAwesome;

namespace homnayangiApp.CustomControls
{
    public class tagControl : BaseViewModel
    {
        private string tagName = string.Empty;
        private bool isChecked = false;
        private SolidColorBrush colorTag = new SolidColorBrush(Color.FromRgb(179, 39, 28));
        private FontImageSource img = new FontImageSource { Color = Color.FromRgb(254, 218, 93),
        FontFamily = "FASolid", Glyph = Solid.Circle};

        public string TagName { get => tagName; set => SetProperty(ref tagName, value); }
        public bool IsChecked { get => isChecked; 
            set 
            { 
                SetProperty(ref isChecked, value);
                if (value)
                {
                    Img = new FontImageSource
                    {
                        Color = Color.FromRgb(170, 245, 119),
                        FontFamily = "FASolid",
                        Glyph = Solid.CircleCheck
                    };
                    ColorTag = new SolidColorBrush(Color.FromRgb(39, 22, 108));
                }
                else
                {
                    Img = new FontImageSource
                    {
                        Color = Color.FromRgb(254, 218, 93),
                        FontFamily = "FASolid",
                        Glyph = Solid.Circle
                    };
                    ColorTag = new SolidColorBrush(Color.FromRgb(179, 39, 28));
                }
            } 
        }
        public SolidColorBrush ColorTag { get => colorTag; set => SetProperty(ref colorTag, value); }
        public FontImageSource Img { get => img; set => SetProperty(ref img, value); }

        public DelegateCommand changeCheck { get; }
        public tagControl()
        {
            changeCheck = new DelegateCommand(executeCheckChange);
        }

        private void executeCheckChange()
        {
            IsChecked = !IsChecked;
        }
    }
}
