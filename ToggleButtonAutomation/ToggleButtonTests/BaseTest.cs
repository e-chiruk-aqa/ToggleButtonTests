using FlaUI.Core.AutomationElements;
using Framework;
using Framework.Utils;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ToggleButtonTests
{
    public abstract class BaseTest
    {
        protected record Bin(string Name, string Path);
        protected TimeSpan LaunchTimeout, SmartScreenTimeout;
        protected Bin[] Bins = Array.Empty<Bin>();

        // run-level
        protected string RunDir = "";
        // case-level
        private string _caseName = "";
        private readonly List<(string label, Bitmap bmp)> _shots = new();

        protected AppLauncher? _launcher;
        protected UiSession? _session;

        [OneTimeSetUp]
        public void OneTimeSetup_Base()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            RunDir = Path.Combine("reports", $"run-{timestamp}");
            Directory.CreateDirectory(RunDir);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(RunDir, "log.txt"))
                .CreateLogger();

            var cfg = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            LaunchTimeout = TimeSpan.FromSeconds(cfg.GetSection("Timeouts:LaunchSec").Get<int>());
            SmartScreenTimeout = TimeSpan.FromSeconds(cfg.GetSection("Timeouts:SmartScreenSec").Get<int>());
            Bins = cfg.GetSection("Binaries").Get<Bin[]>() ?? Array.Empty<Bin>();
            Log.Information("===== Test run started: {RunDir} =====", RunDir);
        }

        protected System.Collections.IEnumerable Cases()
        {
            foreach (var b in Bins)
                yield return new TestCaseData(b.Name, Path.GetFullPath(b.Path)).SetName($"Toggle_{b.Name}");
        }

        protected void BeginCase(string name)
        {
            _caseName = name;
            _shots.Clear();
        }

        protected void AddShot(string label, Bitmap bmp) => _shots.Add((label, bmp));

        protected Window StartAppAndGetWindow(string name, string exePath)
        {
            BeginCase(name);
            Log.Information("=== {Name} ===", name);

            _launcher = new AppLauncher();
            _launcher.Launch(exePath);
            SmartScreenBypass.TryBypass(SmartScreenTimeout);

            _session = new UiSession();
            return _session.AttachToMainWindow(LaunchTimeout);
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            Log.Information("===== All tests finished =====");
            Log.CloseAndFlush();
        }

        [TearDown]
        public void TearDown_Base()
        {
            try
            {
                var ctx = TestContext.CurrentContext;
                string status = ctx.Result.Outcome.Status.ToString();
                string msg = ctx.Result.Message ?? "";

                Log.Information("=== {TestName} finished with {Status}", ctx.Test.Name, status);

                if (!string.IsNullOrWhiteSpace(msg))
                    Log.Error("Message: {Message}", msg);

                if (ctx.Result.StackTrace is not null)
                    Log.Debug("StackTrace:\n{Stack}", ctx.Result.StackTrace);

                if (!string.IsNullOrEmpty(_caseName) && _shots.Count > 0)
                {
                    var dir = Path.Combine(RunDir, _caseName);
                    Directory.CreateDirectory(dir);

                    int i = 0;
                    foreach (var (label, bmp) in _shots)
                    {
                        var fn = Path.Combine(dir, $"{i:00}_{label}.png");
                        bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
                        bmp.Dispose();
                        i++;
                    }
                }
            }
            finally
            {
                try { _session?.Dispose(); } catch { }
                try { _launcher?.Dispose(); } catch { }
                _session = null; _launcher = null;
            }
        }
    }
}
