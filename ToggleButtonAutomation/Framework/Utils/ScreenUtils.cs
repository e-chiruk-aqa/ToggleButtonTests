using FlaUI.Core.AutomationElements;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Framework.Utils
{
    public static class ScreenUtils
    {
        private static Rectangle ToRect(Rectangle r) =>
            new((int)Math.Round((decimal)r.Left),
                (int)Math.Round((decimal)r.Top),
                (int)Math.Round((decimal)r.Width),
                (int)Math.Round((decimal)r.Height));

        public static Bitmap CaptureWindow(Window w)
        {
            var wr = ToRect(w.BoundingRectangle);
            return CaptureScreen(wr);
        }

        public static Bitmap CaptureRoi(Rectangle roiScreen)
        {
            return CaptureScreen(roiScreen);
        }

        public static Rectangle MakeRoiInWindow(Window window, Rectangle roiInWindow)
        {
            var wb = ToRect(window.BoundingRectangle);

            // DPI и округление
            int x = (int)Math.Round((decimal)(wb.X + roiInWindow.X));
            int y = (int)Math.Round((decimal)(wb.Y + roiInWindow.Y));
            int w = (int)Math.Round((decimal)roiInWindow.Width);
            int h = (int)Math.Round((decimal)roiInWindow.Height);

            return new Rectangle(x, y, w, h);
        }

        public static Rectangle MakeRoiFullWindow(Window window)
        {
            return ToRect(window.BoundingRectangle);
        }

        public static Bitmap CaptureScreen(Rectangle screenRect)
        {
            var bmp = new Bitmap(screenRect.Width, screenRect.Height, PixelFormat.Format24bppRgb);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen(screenRect.Left, screenRect.Top, 0, 0, screenRect.Size);
            return bmp;
        }
    }
}
