using DayDaily.Common;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace DayDaily.Controls
{
    public class UserTimer : FrameworkElement
    {
        public static readonly DependencyProperty MaxTimerProperty = DependencyProperty.Register(
            "MaxTimer",
            typeof(int),
            typeof(UserTimer),
            new FrameworkPropertyMetadata(
                180,
                FrameworkPropertyMetadataOptions.AffectsRender));
        public int MaxTimer { get => (int)GetValue(MaxTimerProperty); set => SetValue(MaxTimerProperty, value); }

        public static readonly DependencyProperty CurrentClockProperty = DependencyProperty.Register(
            "CurrentClock",
            typeof(int),
            typeof(UserTimer),
            new FrameworkPropertyMetadata(
                180,
                FrameworkPropertyMetadataOptions.AffectsRender));
        public int CurrentClock { get => (int)GetValue(CurrentClockProperty); set => SetValue(CurrentClockProperty, value); }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Brush),
            typeof(UserTimer),
            new FrameworkPropertyMetadata(Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush Fill { get => (Brush)GetValue(FillProperty); set => SetValue(FillProperty, value); }

        public static readonly DependencyProperty OverTimeFillProperty = DependencyProperty.Register(
            "OverTimeFill",
            typeof(Brush),
            typeof(UserTimer),
            new FrameworkPropertyMetadata(Brushes.Red,
                FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush OverTimeFill { get => (Brush)GetValue(OverTimeFillProperty); set => SetValue(OverTimeFillProperty, value); }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground",
            typeof(Brush),
            typeof(UserTimer),
            new FrameworkPropertyMetadata(Brushes.White,
                FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush Foreground { get => (Brush)GetValue(ForegroundProperty); set => SetValue(ForegroundProperty, value); }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            StreamGeometry geometry = new StreamGeometry();
            bool isOverTime = CurrentClock >= MaxTimer;
            Brush fillColor = isOverTime ? OverTimeFill : Fill;
            var currentClock = CurrentClock % MaxTimer;
            using (var context = geometry.Open())
            {
                DrawGeometry(context, currentClock);
            }
            geometry.Freeze();
            if (isOverTime)
            {
                var radius = GetRadius();
                drawingContext.DrawEllipse(Fill, null, GetCenter(), radius, radius);
            }
            drawingContext.DrawGeometry(fillColor, null, geometry);

            var text = new FormattedText(CurrentClock.ToString(),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                32,
                Foreground);
            var textOrigin = GetCenter();
            textOrigin.X -= text.Width / 2;
            textOrigin.Y -= text.Height / 2;
            drawingContext.DrawText(text, textOrigin);
        }

        private Point GetCenter()
        {
            double width = Width != 0 ? Width : ActualWidth;
            double height = Height != 0 ? Height : ActualHeight;
            return new Point(width / 2, height / 2);
        }

        private double GetRadius()
        {
            var center = GetCenter();
            return Math.Min(center.X, center.Y);
        }

        private void DrawGeometry(StreamGeometryContext context, int currentClock)
        {
            double angle = (double)currentClock / MaxTimer * 360;
            double radius = GetRadius();
            bool largeArc = angle > 180.0;

            Point center = GetCenter();
            Point startPoint = new Point(center.X, center.Y - radius);
            Point endPoint = Utils.ComputeCartesianCoordinate(angle, radius);
            endPoint.X += center.X;
            endPoint.Y += center.Y;
            Size arcSize = new Size(radius, radius);

            context.BeginFigure(center, true, true);
            context.LineTo(startPoint, true, true);
            context.ArcTo(endPoint, arcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
            context.LineTo(center, true, true);
        }
    }
}
