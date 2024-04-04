using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.CustomControls
{
    public class ConvertMoney : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is long totalMoney)
            {
                return string.Format("{0:0,0} đ", totalMoney);
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string totalMoney)
            {
                string money = "";
                if (totalMoney == "")
                    return 0;
                else
                {
                    foreach (var item in totalMoney)
                    {
                        if (char.IsDigit(item))
                        {
                            money += item;
                        }
                    }
                }
                return long.Parse(money);
            }
            return value;
        }
    }
}
