using FlaUI.Core.AutomationElements;

namespace ToggleButtonTests.Pages
{
    public class BaseWindow
    {
        public readonly Window Window;
        public BaseWindow(Window window) => Window = window;

        public Rectangle WinRect() =>
            new Rectangle((int)Window.BoundingRectangle.Left, (int)Window.BoundingRectangle.Top,
                          (int)Window.BoundingRectangle.Width, (int)Window.BoundingRectangle.Height);

        public Point CenterPoint() =>
            new Point(WinRect().X + WinRect().Width / 2, WinRect().Y + WinRect().Height / 2);

        public static void Click(Point p)
        {
            FlaUI.Core.Input.Mouse.MoveTo(p);
            FlaUI.Core.Input.Mouse.Click(p);
        }

        public void MoveTo(Point p)
        {
            FlaUI.Core.Input.Mouse.MoveTo(p);
        }

        public void ClickCenter()
        {
            var p = CenterPoint();
            Click(p);
        }
    }
}
