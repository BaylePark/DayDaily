using System.Collections.Generic;

namespace DayDaily.Model.VO
{
    public class DeveloperInfo
    {
        public UserInfo UserInfo { get; private set; }
        public IList<JiraItem> JiraItems { get; private set; } = new List<JiraItem>();

        public DeveloperInfo(UserInfo userInfo)
        {
            UserInfo = userInfo;
        }
    }
}
