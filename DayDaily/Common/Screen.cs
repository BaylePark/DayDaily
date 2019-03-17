using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;

namespace DayDaily.Common
{
    public class Screen
    {
        public static IEnumerable<Screen> AllScreens()
        {
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                yield return new Screen(screen);
            }
        }

        public static int GetScreenCount()
        {
            return System.Windows.Forms.Screen.AllScreens.Length;
        }

        public static Screen GetScreenFromIndex(int index)
        {
            return new Screen(System.Windows.Forms.Screen.AllScreens[index]);
        }

        public static Screen GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            var screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            Screen wpfScreen = new Screen(screen);
            return wpfScreen;
        }

        public static Screen GetScreenFrom(System.Windows.Point point)
        {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            // are x,y device-independent-pixels ??
            System.Drawing.Point drawingPoint = new System.Drawing.Point(x, y);
            var screen = System.Windows.Forms.Screen.FromPoint(drawingPoint);
            Screen wpfScreen = new Screen(screen);

            return wpfScreen;
        }

        public static Screen Primary
        {
            get { return new Screen(System.Windows.Forms.Screen.PrimaryScreen); }
        }

        private readonly System.Windows.Forms.Screen screen_;

        internal Screen(System.Windows.Forms.Screen screen)
        {
            screen_ = screen;
        }

        public Rect DeviceBounds
        {
            get { return GetRect(screen_.Bounds); }
        }

        public Rect WorkingArea
        {
            get { return GetRect(screen_.WorkingArea); }
        }

        private Rect GetRect(Rectangle value)
        {
            // should x, y, width, height be device-independent-pixels ??
            return new Rect
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
            };
        }

        public bool IsPrimary
        {
            get { return screen_.Primary; }
        }

        public string DeviceName
        {
            get { return screen_.DeviceName; }
        }
    }
}
