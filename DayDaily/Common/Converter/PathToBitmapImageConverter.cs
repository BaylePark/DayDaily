using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DayDaily.Common.Converter
{
    public class PathToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(Application.Current is App))
            {
                var path = Path.Combine(@"C:\Users\hun40\Documents\GitHub\DayDaily\DayDaily\bin\Debug\picture", (string)value + (string)parameter);
                if (!File.Exists(path))
                {
                    return null;
                }
                return new BitmapImage(new Uri(path));
            }
            else
            {
                var path = Path.Combine(Environment.CurrentDirectory, @"picture", (string)value + (string)parameter);
                if (!File.Exists(path))
                {
                    return null;
                }
                return new BitmapImage(new Uri(path));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
