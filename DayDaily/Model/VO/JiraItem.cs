namespace DayDaily.Model.VO
{
    public class JiraItem
    {
        public string Title { get; private set; }

        public JiraItem(string title)
        {
            Title = title;
        }
    }
}
