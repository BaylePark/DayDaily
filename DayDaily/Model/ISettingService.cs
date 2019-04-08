using DayDaily.Model.VO;
using System.Collections.Generic;
using System.Windows;

namespace DayDaily.Model
{
    public interface ISettingService
    {
        DisplayDeviceInfo LastScreenInfo { get; set; }
        
        void SaveSettings();
    }
}
