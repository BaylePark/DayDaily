namespace DayDaily.Model.VO
{
    public enum JiraItemType
    {
        Epic,
        Story,
        Task,
        SubTask,
        Bug,
    }

    public enum JiraItemStatus
    {
        Backlog,
        ToDo,
        InProgress,
        Pending,
        Done,
    }

    public class JiraItem
    {
        public string Key { get; private set; }
        public string Title { get; private set; }
        public JiraItemType Type { get; set; }
        public JiraItemStatus Status { get; set; }
        public string EpicTitle { get; set; }
        public UserInfo User { get; private set; }

        public JiraItem(string key, string title, UserInfo user)
        {
            Key = key;
            Title = title;
            User = user;
        }
    }
}
