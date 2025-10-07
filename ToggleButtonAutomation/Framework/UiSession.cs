using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace Framework
{
    public sealed class UiSession : IDisposable
    {
        public Application? App { get; private set; }
        public UIA3Automation Automation { get; } = new();

        public Window AttachToMainWindow(TimeSpan timeout)
        {
            var end = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < end)
            {
                var a = Automation.GetDesktop();
                var b = Automation.GetDesktop().FindAllChildren();
                var wins = Automation.GetDesktop().FindAllChildren().ToList();
                var target = wins.FirstOrDefault(w => w.Name.Length > 0 && w.Name.Contains("Kzb Player"));
                if (target != null) return target.AsWindow();
                Thread.Sleep(200);
            }
            throw new AssertionException("Application did not launch: main window not found");
        }

        public void Dispose()
        {
            Automation.Dispose();
            try { App?.Close(); } catch { }
        }
    }
}
