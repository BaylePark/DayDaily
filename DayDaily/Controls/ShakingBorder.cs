using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DayDaily.Controls
{
    public class ShakingBorder : Border
    {
        static Random _random = new Random();

        public static readonly DependencyProperty IsShakingProperty =
            DependencyProperty.Register("IsShaking", typeof(bool), typeof(ShakingBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None,
                (s, e) =>
                {
                    var shakingBorder = s as ShakingBorder;
                    if (shakingBorder == null) return;

                    shakingBorder.keyframes.Duration = TimeSpan.FromMilliseconds(_random.Next(200) + 100);
                    shakingBorder.keyframes.KeyFrames.Add(shakingBorder.StartAnimation);
                    shakingBorder.keyframes.KeyFrames.Add(shakingBorder.EndAnimation);
                    shakingBorder.keyframes.AutoReverse = true;
                    shakingBorder.keyframes.RepeatBehavior = RepeatBehavior.Forever;

                    var rt = (RotateTransform)shakingBorder.RenderTransform;
                    rt.BeginAnimation(RotateTransform.AngleProperty, shakingBorder.keyframes);
                }));

        public bool IsShaking { get => (bool)GetValue(IsShakingProperty); set => SetValue(IsShakingProperty, value); }

        public static readonly DependencyProperty ShakingFactorProperty =
            DependencyProperty.Register("ShakingFactor", typeof(double), typeof(ShakingBorder), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None,
                (s, e) =>
                {
                    var shakingBorder = s as ShakingBorder;
                    if (shakingBorder == null) return;

                    shakingBorder.StartAnimation.Value = (double)e.NewValue * -1;
                    shakingBorder.EndAnimation.Value = (double)e.NewValue;
                    var rt = (RotateTransform)shakingBorder.RenderTransform;
                    rt.BeginAnimation(RotateTransform.AngleProperty, shakingBorder.keyframes, HandoffBehavior.SnapshotAndReplace);
                }));

        public double ShakingFactor { get => (double)GetValue(ShakingFactorProperty); set => SetValue(ShakingFactorProperty, value); }

        DoubleAnimationUsingKeyFrames keyframes = new DoubleAnimationUsingKeyFrames();
        EasingDoubleKeyFrame StartAnimation { get; set; }
        EasingDoubleKeyFrame EndAnimation { get; set; }

        public ShakingBorder()
        {
            RenderTransformOrigin = new Point(.5, .5);
            RenderTransform = new RotateTransform(0);
            StartAnimation = new EasingDoubleKeyFrame(ShakingFactor * -1, KeyTime.FromPercent(0.5), new SineEase() { EasingMode = EasingMode.EaseIn });
            EndAnimation = new EasingDoubleKeyFrame(ShakingFactor, KeyTime.FromPercent(1.0), new SineEase() { EasingMode = EasingMode.EaseOut });
        }
    }
}
