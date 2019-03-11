using System.Collections.Generic;

namespace DayDaily.Model
{
    public class User
    {
        public string Name { get; private set; }
        public IList<JiraItem> JiraItems { get; private set; } = new List<JiraItem>();

        public User(string name)
        {
            Name = name;
        }
    }
}
