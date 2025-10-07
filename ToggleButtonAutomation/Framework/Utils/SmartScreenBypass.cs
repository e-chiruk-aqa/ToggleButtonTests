using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using System;
using System.Linq;
using System.Threading;

namespace Framework.Utils
{
    public static class SmartScreenBypass
    {
        public static void TryBypass(TimeSpan timeout)
        {
            using var automation = new UIA3Automation();
            var end = DateTime.UtcNow + timeout;

            while (DateTime.UtcNow < end)
            {
                var win = RetryFind(() =>
                {
                    var windows = automation.GetDesktop().FindAllChildren().OfType<Window>();
                    return windows.FirstOrDefault(w =>
                        w.Title.Contains("Windows protected your PC", StringComparison.OrdinalIgnoreCase) ||
                        w.Title.Contains("Microsoft Defender SmartScreen", StringComparison.OrdinalIgnoreCase));
                }, 200);

                if (win == null) { Thread.Sleep(200); continue; }

                var more = win.FindFirstDescendant(cf =>
                    cf.ByControlType(ControlType.Hyperlink).Or(cf.ByName("More info")));
                more?.Click();

                Thread.Sleep(300);

                var run = win.FindFirstDescendant(cf => cf.ByName("Run anyway"));
                run?.Click();

                Thread.Sleep(500);
                return;
            }
        }

        static T? RetryFind<T>(Func<T?> f, int pauseMs)
        {
            for (int i = 0; i < 5; i++) { var r = f(); if (r != null) return r; Thread.Sleep(pauseMs); }
            return default;
        }
    }
}