using System.Windows;
using System.Windows.Controls;

namespace DayDaily.Common.Behaviors
{
    public class ScrollViewerBehavior
    {
        public static readonly DependencyProperty VerticalScrollOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalScrollOffset", typeof(double), typeof(ScrollViewerBehavior),
                new PropertyMetadata(0.0, (o, e) =>
                    {
                        ScrollViewer scrollViewer = o as ScrollViewer;
                        if (scrollViewer == null)
                        {
                            return;
                        }

                        scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
                    }));

        public static double GetVerticalScrollOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalScrollOffsetProperty);
        }

        public static void SetVerticalScrollOffset(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalScrollOffsetProperty, value);
        }
    }
}
