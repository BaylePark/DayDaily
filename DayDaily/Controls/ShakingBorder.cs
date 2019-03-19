using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DayDaily.Controls
{
    public class ShakingBorder : Border
    {
        public static readonly DependencyProperty IsShakingProperty =
            DependencyProperty.Register("IsShaking", typeof(bool), typeof(ShakingBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, IsShakingPropertyChanged));

        static Random _random = new Random();

        static void IsShakingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var shakingBorder = d as ShakingBorder;

            var keyframes = new DoubleAnimationUsingKeyFrames();
            keyframes.Duration = TimeSpan.FromMilliseconds(_random.Next(200) + 100);
            var startAnimation = new EasingDoubleKeyFrame(-1, KeyTime.FromPercent(0.5), new SineEase() { EasingMode = EasingMode.EaseIn });
            var endAnimation = new EasingDoubleKeyFrame(1, KeyTime.FromPercent(1.0), new SineEase() { EasingMode = EasingMode.EaseOut });
            keyframes.KeyFrames.Add(startAnimation);
            keyframes.KeyFrames.Add(endAnimation);
            keyframes.AutoReverse = true;
            keyframes.RepeatBehavior = RepeatBehavior.Forever;

            var rt = (RotateTransform)shakingBorder.RenderTransform;
            rt.BeginAnimation(RotateTransform.AngleProperty, keyframes);
        }

        public bool IsShaking { get => (bool)GetValue(IsShakingProperty); set => SetValue(IsShakingProperty, value); }

        public ShakingBorder()
        {
            RenderTransformOrigin = new Point(.5, .5);
            RenderTransform = new RotateTransform(0);
        }
    }
}
