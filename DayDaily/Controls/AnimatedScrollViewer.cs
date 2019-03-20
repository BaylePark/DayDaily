using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DayDaily.Controls
{
    public class AnimatedScrollViewer : ScrollViewer
    {
        private readonly static DependencyProperty ScrollProperty = DependencyProperty.Register(
            "Scroll", typeof(double), typeof(AnimatedScrollViewer), new PropertyMetadata(0.0, (o, e) =>
            {
                AnimatedScrollViewer target = o as AnimatedScrollViewer;
                if (target == null) return;

                target.ScrollToVerticalOffset((double)e.NewValue);
            }));

        public readonly static DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register(
            "CurrentVerticalOffset", typeof(double), typeof(AnimatedScrollViewer), new UIPropertyMetadata(0.0, (o, e) =>
                {
                    AnimatedScrollViewer target = o as AnimatedScrollViewer;
                    if (target == null) return;

                    double offset = 0;
                    if ((double)e.NewValue < 0)
                    {
                        offset = target.VerticalOffset - 500;
                    }
                    else
                    {
                        offset = target.VerticalOffset + 500;
                    }

                    var easeFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 8 };
                    var scrollanim = new DoubleAnimation(offset, TimeSpan.FromSeconds(0.2)) { EasingFunction = easeFunction };
                    target.BeginAnimation(ScrollProperty, scrollanim);
                }));

        public double CurrentVerticalOffset
        {
            get => (double)GetValue(CurrentVerticalOffsetProperty);
            set => SetValue(CurrentVerticalOffsetProperty, value);
        }

        public AnimatedScrollViewer()
        {
            PreviewMouseWheel += (s, e) =>
            {
                e.Handled = true;
            };
        }
    }
}
