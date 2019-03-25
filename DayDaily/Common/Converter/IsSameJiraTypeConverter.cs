using DayDaily.Model.VO;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DayDaily.Common.Converter
{
    class IsSameJiraTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (JiraItemType)value == (JiraItemType)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
