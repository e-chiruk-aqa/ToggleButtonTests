using Framework.Extensions;
using Framework.Utils;
using ToggleButtonTests.Pages;
using ToggleButtonTests.Utils;

namespace ToggleButtonTests
{
    [TestFixture]
    public class ToggleTests : BaseTest
    {
        private const double SameThreshold = 0.98;
        private const int PauseMs = 500;

        [TestCaseSource(typeof(TestDataUtil), nameof(TestDataUtil.Binaries))]
        public void Toggle_changes_state(string name, string path)
        {
            var win = StartAppAndGetWindow(name, path);
            var page = new MainWindow(win);

            win.SetForegroundSafe();
            win.MoveTo(100, 100);
            win.ResizeTo(900, 650);

            var roiInWin = page.CenterRoiInWindow();
            var roiScreen = page.MakeRoiInScreen(roiInWin);

            var before = ScreenUtils.CaptureRoi(roiScreen); AddShot("state0", before);
            page.ClickCenter();
            page.MoveTo(new Point());
            var after1 = ScreenUtils.CaptureRoi(roiScreen); AddShot("state1", after1);
            page.ClickCenter();
            page.MoveTo(new Point());
            var after2 = ScreenUtils.CaptureRoi(roiScreen); AddShot("state2", after2);

            double ssim1 = ImageUtils.Ssim(before, after1);
            double ssim2 = ImageUtils.Ssim(before, after2);

            var baseDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData");
            double b0 = ImageUtils.CompareToBaseline(before, Path.Combine(baseDir, "state0.png"),
                                                     Path.Combine(Path.Combine("reports", name), "diff_state0.png"));
            double b1 = ImageUtils.CompareToBaseline(after1, Path.Combine(baseDir, "state1.png"),
                                                     Path.Combine(Path.Combine("reports", name), "diff_state1.png"));
            double b2 = ImageUtils.CompareToBaseline(after2, Path.Combine(baseDir, "state0.png"),
                                                     Path.Combine(Path.Combine("reports", name), "diff_state2.png"));

            Assert.Multiple(() =>
            {
                Assert.That(b0, Is.GreaterThanOrEqualTo(SameThreshold), "Initial ROI differs from baseline");
                Assert.That(b1, Is.GreaterThanOrEqualTo(SameThreshold), "After 1st click ROI differs from baseline");
                Assert.That(b2, Is.GreaterThanOrEqualTo(SameThreshold), "After 2nd click ROI differs from baseline");
            });
        }
    }
}
