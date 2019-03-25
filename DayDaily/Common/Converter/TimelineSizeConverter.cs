using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DayDaily.Common.Converter
{
    public class TimelineSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int duration = (int)values[0];
            int wholeTime = (int)values[1];
            double actualWidth = (double)values[2];
            double percent = (double)duration / wholeTime;
            return actualWidth * percent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
