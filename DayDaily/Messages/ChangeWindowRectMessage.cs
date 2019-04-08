using System.Windows;

namespace DayDaily.Messages
{
    public class ChangeWindowRectMessage
    {
        public Point Location { get; private set; }
        public ChangeWindowRectMessage(Point location)
        {
            Location = location;
        }
    }
}
