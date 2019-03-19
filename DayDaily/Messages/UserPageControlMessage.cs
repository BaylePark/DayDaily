namespace DayDaily.Messages
{
    public enum UserPageControlType
    {
        Prev,
        Next
    }

    public class UserPageControlMessage
    {
        public UserPageControlType Type { get; private set; }

        public UserPageControlMessage(UserPageControlType type)
        {
            Type = type;
        }
    }
}
