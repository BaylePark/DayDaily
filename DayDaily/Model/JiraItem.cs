namespace DayDaily.Model
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
