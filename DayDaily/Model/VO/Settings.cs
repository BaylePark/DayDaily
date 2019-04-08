using Newtonsoft.Json;

namespace DayDaily.Model.VO
{
    [JsonObject("settings", MemberSerialization = MemberSerialization.OptIn)]
    public class Settings
    {
        [JsonProperty("lastDisplayDevice")]
        public DisplayDeviceInfo LastDisplayDevice { get; set; } = null;
    }
}
