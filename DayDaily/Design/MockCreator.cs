using DayDaily.Model.VO;
using System;
using System.Collections.Generic;

namespace DayDaily.Design
{
    static public class MockCreator
    {
        static public void CreateUsers(IDictionary<string, UserInfo> users)
        {
            users.Add("박병훈", new UserInfo() { Name = "박병훈", Belong = "S/W Platform Lab", SingleID = "bayle.park" });
            users.Add("황재경", new UserInfo() { Name = "황재경", Belong = "S/W Platform Lab", SingleID = "jk0719.hwang" });
            users.Add("백종민", new UserInfo() { Name = "백종민", Belong = "S/W Platform Lab", SingleID = "jm.back" });
            users.Add("장병욱", new UserInfo() { Name = "장병욱", Belong = "S/W Platform Lab", SingleID = "bw.jang" });
            users.Add("이지혜", new UserInfo() { Name = "이지혜", Belong = "S/W Platform Lab", SingleID = "jh.lee" });
            users.Add("김현태", new UserInfo() { Name = "김현태", Belong = "S/W Platform Lab", SingleID = "ht.kim" });
        }

        static public void CreateJiraItems(IDictionary<string, UserInfo> users, IList<JiraItem> jiraItems)
        {
            int id = 1;
            var r = new Random();
            foreach (var keyval in users)
            {
                var user = keyval.Value;
                int itemCount = r.Next(10) + 15;

                for (int i = 0; i < itemCount; i++)
                {
                    var status = (JiraItemStatus)(r.Next(5));
                    var type = (JiraItemType)(r.Next(4));

                    string title = "";
                    int titleLen = r.Next(60) + 20;
                    for (int j = 0; j < titleLen; j++)
                    {
                        char c = (char)('a' + (r.Next(24)));
                        title += c;
                    }

                    var jiraitem = new JiraItem("ID-" + id, type.ToString() + " #" + id + " " + title, user)
                    {
                        Status = status,
                        Type = type,
                        EpicTitle = "EPIC #" + r.Next(100),
                    };

                    jiraItems.Add(jiraitem);
                    id++;
                }
            }
        }
    }
}
