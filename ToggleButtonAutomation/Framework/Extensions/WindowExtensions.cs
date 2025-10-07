using FlaUI.Core.AutomationElements;

namespace Framework.Extensions
{
    public static class WindowExtensions
    {
        public static void ResizeTo(this Window w, int width, int height)
        {
            if (w.Patterns.Transform.IsSupported &&
                w.Patterns.Transform.Pattern.CanResize.Value)
            {
                w.Patterns.Transform.Pattern.Resize(width, height);
            }
        }

        public static void MoveTo(this Window w, int x, int y)
        {
            if (w.Patterns.Transform.IsSupported &&
                w.Patterns.Transform.Pattern.CanMove.Value)
            {
                w.Patterns.Transform.Pattern.Move(x, y);
            }
        }

        public static void SetForegroundSafe(this Window w)
        {
            try { w.SetForeground(); } catch { try { w.Focus(); } catch { } }
        }
    }
}
