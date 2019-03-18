using DayDaily.Model;
using DayDaily.Model.VO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayDaily.Design
{
    public class DesignDataService : IDataService
    {
        IDictionary<string, UserInfo> _users = new Dictionary<string, UserInfo>();
        IList<JiraItem> _jiraItems = new List<JiraItem>();
        IList<UserInfo> _orderedUsers = new List<UserInfo>();
        int _currentUserIndex = 0;

        public UserInfo CurrentUser { get; set; }

        public DesignDataService()
        {
            MockCreator.CreateUsers(_users);
            MockCreator.CreateJiraItems(_users, _jiraItems);
            CurrentUser = _users.ElementAt(0).Value;
        }

        public async Task LoadAsync() { }

        public IList<UserInfo> GetAllUserInfos() => new List<UserInfo>(from keyValuePair in _users select keyValuePair.Value);

        public IList<JiraItem> GetJiraItemsByUserName(string name) => new List<JiraItem>(from jiraitem in _jiraItems where jiraitem.User.Name == name select jiraitem);

        public void AddOrderedUser(UserInfo user)
        {
            _orderedUsers.Add(user);
        }

        public void SetNextUser()
        {
            CurrentUser = _orderedUsers[_currentUserIndex];
            _currentUserIndex++;
        }
    }
}