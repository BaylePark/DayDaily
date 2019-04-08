using DayDaily.Model;
using DayDaily.Model.VO;

namespace DayDaily.Design
{
    class DesignSettingService : ISettingService
    {
        public DisplayDeviceInfo LastScreenInfo
        {
            get => new DisplayDeviceInfo() { DeviceKey = "HASH#1" };
            set => throw new System.NotImplementedException();
        }

        public void SaveSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
