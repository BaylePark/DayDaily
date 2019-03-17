using Newtonsoft.Json;

namespace DayDaily.Model.VO
{
    [JsonObject("settings", MemberSerialization = MemberSerialization.OptIn)]
    public class Settings
    {
        [JsonProperty("selectedSecreenIndex")]
        public int SelectedScreenIndex { get; set; } = 0;
    }
}
