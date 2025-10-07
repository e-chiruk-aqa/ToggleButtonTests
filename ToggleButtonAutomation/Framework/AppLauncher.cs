using NUnit.Framework;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;

namespace Framework
{
    public sealed class AppLauncher : IDisposable
    {
        public Process? Process { get; private set; }

        public void Launch(string exePath)
        {
            if (!File.Exists(exePath))
                throw new AssertionException($"Executable not found: {exePath}");

            TryUnblock(exePath);

            var psi = new ProcessStartInfo(exePath)
            {
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(exePath)!
            };
            Process = Process.Start(psi) ?? throw new AssertionException("Failed to start process");
            Log.Information("Started {Exe} pid={Pid}", exePath, Process.Id);
        }

        static void TryUnblock(string path)
        {
            try
            {
                var zoneId = Path.Combine(Path.GetDirectoryName(path)!, Path.GetFileName(path) + ":Zone.Identifier");
                if (File.Exists(zoneId)) File.Delete(zoneId);
            }
            catch (Exception e) { Log.Warning(e, "Unblock failed, will try SmartScreen bypass"); }
        }

        public void Dispose()
        {
            try
            {
                if (Process is { HasExited: false })
                {
                    Process.Kill(entireProcessTree: true);
                    Process.WaitForExit(3000);
                }
            }
            catch { }
        }
    }
}
