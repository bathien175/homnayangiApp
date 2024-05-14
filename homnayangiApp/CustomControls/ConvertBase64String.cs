using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.CustomControls
{
    public class ConvertBase64String : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value!= null)
            {
                var imgt = System.Convert.FromBase64String(value.ToString());
                MemoryStream stream2 = new(imgt);
                ImageSource image = ImageSource.FromStream(() => stream2);
                return image;
            }
            else
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "backgroundstore.jpg");
                return ImageSource.FromFile(imagePath);
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
