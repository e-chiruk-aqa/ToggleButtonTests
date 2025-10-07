using Microsoft.Extensions.Configuration;

namespace ToggleButtonTests.Utils
{
    public static class TestDataUtil
    {
        public static IEnumerable<TestCaseData> Binaries()
        {
            var baseDir = TestContext.CurrentContext.TestDirectory;
            var cfg = new ConfigurationBuilder()
                .SetBasePath(baseDir)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            foreach (var b in cfg.GetSection("Binaries").GetChildren())
            {
                var name = b["Name"]!;
                var path = Path.GetFullPath(Path.Combine(baseDir, b["Path"]!));
                yield return new TestCaseData(name, path).SetName($"Toggle_{name}");
            }
        }
    }
}
