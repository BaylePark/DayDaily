using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DayDaily.Common.Behaviors
{
    public class FocusAfterLoadedBehavior
    {
        public static readonly DependencyProperty FocusingAfterLoadedProperty =
            DependencyProperty.RegisterAttached("FocusAfterLoaded", typeof(bool), typeof(FocusAfterLoadedBehavior), new UIPropertyMetadata(false, OnFocusingAfterLoadedPropertyChanged));

        public static bool GetFocusAfterLoaded(FrameworkElement obj)
        {
            return (bool)obj.GetValue(FocusingAfterLoadedProperty);
        }

        public static void SetFocusAfterLoaded(FrameworkElement obj, bool value)
        {
            obj.SetValue(FocusingAfterLoadedProperty, value);
        }

        private static void OnFocusingAfterLoadedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FrameworkElement)d).Loaded += (sender, arg) =>
            {
                var element = (sender as FrameworkElement);
                element.Focusable = true;
                element.Focus();

                var adorner = AdornerLayer.GetAdornerLayer(element);
                adorner.Visibility = Visibility.Hidden;
            };
        }
    }
}
