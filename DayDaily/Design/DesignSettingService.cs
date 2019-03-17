using DayDaily.Model;
using DayDaily.Model.VO;
using System.Collections.Generic;
using System.Windows;

namespace DayDaily.Design
{
    class DesignSettingService : ISettingService
    {
        public int SelectedScreenIndex { get; set; } = 0;

        public Rect GetWindowRectFromIndex(int index)
        {
            return new Rect(0, 0, 600, 480);
        }

        public IEnumerable<ScreenInfo> GetAllScreens()
        {
            ScreenInfo[] infos = new ScreenInfo[]
            {
                new ScreenInfo() {Index = 1, IsPrimary= true, Resolution = new Size(1920, 1080), DeviceName="Bayle"},
                new ScreenInfo() {Index = 2, IsPrimary= false, Resolution = new Size(3840, 2160), DeviceName="LG"},
                new ScreenInfo() {Index = 3, IsPrimary= false, Resolution = new Size(2440, 1380), DeviceName="Samsung"}
            };
            foreach (var screenInfo in infos)
            {
                yield return screenInfo;
            }
        }

        public void SaveSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
