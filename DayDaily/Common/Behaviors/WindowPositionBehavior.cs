using System.Windows;

namespace DayDaily.Common.Behaviors
{
    class WindowPositionBehavior
    {
        public static readonly DependencyProperty WorkingAreaProperty =
            DependencyProperty.RegisterAttached("WorkingArea", typeof(Point), typeof(WindowPositionBehavior),
                new PropertyMetadata(default(Point), (s, e) =>
                {
                    Window window = s as Window;
                    Point rect = (Point)e.NewValue;
                    window.WindowState = WindowState.Normal;
                    window.Left = rect.X;
                    window.Top = rect.Y;
                    window.WindowState = WindowState.Maximized;
                }));

        public static Point GetWorkingArea(FrameworkElement obj)
        {
            return (Point)obj.GetValue(WorkingAreaProperty);
        }
        public static void SetWorkingArea(FrameworkElement obj, Point value)
        {
            obj.SetValue(WorkingAreaProperty, value);
        }
    }
}
