using ImageMagick;
using System.Drawing;
using System.IO;

namespace Framework.Utils
{
    public static class BaselineUtils
    {
        public static void ResizeBaseline(string srcPng, string dstPng, int w, int h)
        {
            using var img = new MagickImage(srcPng) { ColorSpace = ColorSpace.sRGB };
            img.FilterType = FilterType.Triangle;
            img.Resize((uint)w, (uint)h);
            Directory.CreateDirectory(Path.GetDirectoryName(dstPng)!);
            img.Write(dstPng);
        }

        public static void ResizeSet(string baseDir, Rectangle roiSize, string s0 = "state0.png", string s1 = "state1.png")
        {
            int w = roiSize.Width, h = roiSize.Height;
            ResizeBaseline(Path.Combine(baseDir, s0), Path.Combine(baseDir, s0), w, h);
            ResizeBaseline(Path.Combine(baseDir, s1), Path.Combine(baseDir, s1), w, h);
        }
    }
}
