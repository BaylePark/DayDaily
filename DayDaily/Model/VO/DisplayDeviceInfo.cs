using System.Windows;

namespace DayDaily.Model.VO
{
    public class DisplayDeviceInfo
    {
        public int DeviceAccessID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceString { get; set; }
        public string DeviceID { get; set; }
        public string DeviceKey { get; set; }
        public bool IsPrimary { get; set; }

        public Rect Region { get; set; }
    }
}
