using System.Windows;

namespace DayDaily.Model.VO
{
    public class ScreenInfo
    {
        public int Index { get; set; }
        public string DeviceName { get; set; }
        public bool IsPrimary { get; set; }
        public Size Resolution { get; set; }
    }
}
