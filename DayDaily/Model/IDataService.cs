using DayDaily.Model.VO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayDaily.Model
{
    public interface IDataService : IDisposable
    {
        UserInfo CurrentUser { get; set; }

        Task LoadAsync();
        IList<UserInfo> GetAllUserInfos();
        IList<JiraItem> GetJiraItemsByUserName(string name);
        void AddOrderedUser(UserInfo user);
        void SetUserByOrder(int index);
    }
}
