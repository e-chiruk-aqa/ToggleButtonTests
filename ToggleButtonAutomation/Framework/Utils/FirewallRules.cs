using System.Diagnostics;

namespace Framework.Utils;
public static class FirewallRules
{
    public static void EnsureAllowFor(string exePath, string ruleName)
    {
        Run($"advfirewall firewall add rule name=\"{ruleName}-in\" dir=in action=allow program=\"{exePath}\" enable=yes profile=any");
        Run($"advfirewall firewall add rule name=\"{ruleName}-out\" dir=out action=allow program=\"{exePath}\" enable=yes profile=any");
    }

    static void Run(string args)
    {
        var psi = new ProcessStartInfo("netsh", args)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using var p = Process.Start(psi)!;
        p.WaitForExit(10_000);
    }
}
