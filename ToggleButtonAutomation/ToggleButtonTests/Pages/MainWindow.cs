using FlaUI.Core.AutomationElements;

namespace ToggleButtonTests.Pages
{
    public sealed class MainWindow : BaseWindow
    {
        public MainWindow(Window window) : base(window) { }

        public Rectangle CenterRoiInWindow()
        {
            var br = Window.BoundingRectangle;
            int W = (int)Math.Round((decimal)br.Width);
            int H = (int)Math.Round((decimal)br.Height);


            int rw = (int)(W * 0.40);
            int rh = (int)(H * 0.30);


            int xLocal = (W - rw) / 2 + (int)(W * 0.14);
            int yLocal = (H - rh) / 2 + (int)(H * 0.16);

            return new Rectangle(xLocal, yLocal, rw, rh);
        }

        public Rectangle MakeRoiInScreen(Rectangle roiInWindow)
        {
            var b = Window.BoundingRectangle;
            int x = (int)Math.Round((decimal)b.Left) + roiInWindow.X;
            int y = (int)Math.Round((decimal)b.Top) + roiInWindow.Y;
            return new Rectangle(x, y, roiInWindow.Width, roiInWindow.Height);
        }
    }
}
