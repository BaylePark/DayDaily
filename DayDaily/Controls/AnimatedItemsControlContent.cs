using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DayDaily.Controls
{
    public class AnimatedItemsControlContent : ContentControl
    {
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.RegisterAttached(
            "Storyboard", typeof(Storyboard), typeof(AnimatedItemsControlContent), new UIPropertyMetadata(null));

        public static readonly DependencyProperty OpeningEffectsOffsetProperty = DependencyProperty.Register(
           "OpeningEffectsOffset", typeof(TimeSpan), typeof(AnimatedItemsControlContent), new PropertyMetadata(default(TimeSpan)));

        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
           "Delay", typeof(TimeSpan), typeof(AnimatedItemsControlContent), new PropertyMetadata(default(TimeSpan)));

        public Storyboard Storyboard
        {
            get => GetValue(StoryboardProperty) as Storyboard;
            set => SetValue(StoryboardProperty, value);
        }

        public TimeSpan OpeningEffectsOffset
        {
            get => (TimeSpan)GetValue(OpeningEffectsOffsetProperty);
            set => SetValue(OpeningEffectsOffsetProperty, value);
        }

        public TimeSpan Delay
        {
            get => (TimeSpan)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        public override void OnApplyTemplate()
        {
            FrameworkElement nameScopeRoot = GetNameScopeRoot();

            base.OnApplyTemplate();

            RunOpeningEffect();
        }

        private void RunOpeningEffect()
        {
            var storyboard = Storyboard.Clone();
            ContentPresenter cp = GetNameScopeRoot() as ContentPresenter;
            storyboard.BeginTime = Delay + OpeningEffectsOffset;
            storyboard.Begin(cp.Content as FrameworkElement);
        }

        private FrameworkElement GetNameScopeRoot()
        {
            if (VisualChildrenCount > 0 && GetVisualChild(0) is FrameworkElement fe && NameScope.GetNameScope(fe) != null)
            {
                return fe;
            }

            if (NameScope.GetNameScope(this) == null)
            {
                NameScope.SetNameScope(this, new NameScope());
            }

            return this;
        }
    }
}
