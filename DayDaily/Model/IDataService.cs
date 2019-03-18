using DayDaily.Model.VO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayDaily.Model
{
    public interface IDataService
    {
        UserInfo CurrentUser { get; set; }

        Task LoadAsync();
        IList<UserInfo> GetAllUserInfos();
        IList<JiraItem> GetJiraItemsByUserName(string name);
        void AddOrderedUser(UserInfo user);
        void SetNextUser();
    }
}
