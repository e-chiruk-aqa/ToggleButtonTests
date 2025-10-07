using ImageMagick;
using System.Drawing;
using System.IO;

namespace Framework.Utils
{
    public static class ImageUtils
    {
        public static void EnsureDir(string path) =>
            Directory.CreateDirectory(path);

        public static void SavePng(Bitmap bmp, string path)
        {
            EnsureDir(Path.GetDirectoryName(path)!);
            bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static double Ssim(Bitmap a, Bitmap b)
        {
            using var ma = ToMagick(a);
            using var mb = ToMagick(b);
            if (ma.Width != mb.Width || ma.Height != mb.Height) ma.Resize(mb.Width, mb.Height);
            double diff = ma.Compare(mb, ErrorMetric.StructuralSimilarity, Channels.All);
            return 1.0 - diff;
        }

        public static double CompareToBaseline(Bitmap actualRoi, string baselinePath, string diffOutPath)
        {
            using var actual = ToMagick(actualRoi);
            using var baseline = new MagickImage(baselinePath) { ColorSpace = ColorSpace.sRGB };
            if (actual.Width != baseline.Width || actual.Height != baseline.Height)
                actual.Resize(baseline.Width, baseline.Height);

            double diff = baseline.Compare(actual, ErrorMetric.StructuralSimilarity, Channels.All);
            using var dimg = (MagickImage)baseline.Clone();
            dimg.Composite(actual, CompositeOperator.Difference, Channels.All);
            dimg.AutoLevel();
            Directory.CreateDirectory(Path.GetDirectoryName(diffOutPath)!);
            dimg.Write(diffOutPath);
            return 1.0 - diff;
        }

        public static MagickImage ToMagick(Bitmap bmp)
        {
            using var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            return new MagickImage(ms) { ColorSpace = ColorSpace.sRGB };
        }
    }
}
