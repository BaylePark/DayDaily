using DayDaily.Design;
using DayDaily.Model.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayDaily.Model
{
    public class DataService : IDataService
    {
        readonly IDictionary<string, UserInfo> _users = new Dictionary<string, UserInfo>();
        readonly IList<JiraItem> _jiraItems = new List<JiraItem>();
        IList<UserInfo> _orderedUsers = new List<UserInfo>();

        public UserInfo CurrentUser { get; set; }

        public async Task LoadAsync()
        {
            await Task.Run(() =>
            {
                MockCreator.CreateUsers(_users);
                MockCreator.CreateJiraItems(_users, _jiraItems);
            });
        }

        public IList<UserInfo> GetAllUserInfos() => new List<UserInfo>(from keyValuePair in _users select keyValuePair.Value);

        public IList<JiraItem> GetJiraItemsByUserName(string name) => new List<JiraItem>(from jiraitem in _jiraItems where jiraitem.User.Name == name select jiraitem);
        
        public void AddOrderedUser(UserInfo user)
        {
            _orderedUsers.Add(user);
        }

        public void SetUserByOrder(int index)
        {
            CurrentUser = _orderedUsers[index];
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~DataService() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}