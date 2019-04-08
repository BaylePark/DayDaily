using System.Windows;

namespace DayDaily.Common.Behaviors
{
    class WindowPositionBehavior
    {
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.RegisterAttached("Location", typeof(Point), typeof(WindowPositionBehavior),
                new PropertyMetadata(default(Point), (s, e) =>
                {
                    Window window = s as Window;
                    Point rect = (Point)e.NewValue;
                    window.WindowState = WindowState.Normal;
                    window.Left = rect.X;
                    window.Top = rect.Y;
                    window.WindowState = WindowState.Maximized;
                }));

        public static Point GetLocation(FrameworkElement obj)
        {
            return (Point)obj.GetValue(LocationProperty);
        }
        public static void SetLocation(FrameworkElement obj, Point value)
        {
            obj.SetValue(LocationProperty, value);
        }
    }
}
