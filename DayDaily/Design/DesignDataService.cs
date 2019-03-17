using DayDaily.Model;
using DayDaily.Model.VO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayDaily.Design
{
    public class DesignDataService : IDataService
    {
        public Task LoadAsync()
        {
            return null;
        }

        public IList<UserInfo> GetAllUserInfos()
        {
            UserInfo[] userInfos = new UserInfo[]
            {
                new UserInfo() { Name = "박병훈" },
                new UserInfo() { Name = "황재경" },
                new UserInfo() { Name = "김현태" },
                new UserInfo() { Name = "백종민" },
                new UserInfo() { Name = "이지혜" },
                new UserInfo() { Name = "장병욱" },
            };
            return new List<UserInfo>(userInfos);
        }

        public DeveloperInfo GetCurrentDeveloperInfo()
        {
            var developer = new DeveloperInfo(new UserInfo() { Name = "박병훈" });
            developer.JiraItems.Add(new JiraItem("New Jira Item #1"));
            developer.JiraItems.Add(new JiraItem("New Jira Item #2"));
            developer.JiraItems.Add(new JiraItem("New Jira Item #3"));
            developer.JiraItems.Add(new JiraItem("New Jira Item #4"));
            return developer;
        }

        public void SetCurrentDeveloper(string name)
        {
            throw new System.NotImplementedException();
        }

        public string GetWelcomeMessage()
        {
            return @"안녕하세요!" + Environment.NewLine + @"오늘도 즐거운 하루 되세요!";
        }
    }
}