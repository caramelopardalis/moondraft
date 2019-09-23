using System.Collections.Generic;

namespace moondraft.Services
{
    public class DetectFileTypeService
    {
        static HashSet<string> ImageExtensions = new HashSet<string>
        {
            "jpg",
            "jpeg",
            "jp2",
            "j2c",
            "jxr",
            "hdp",
            "wdp",
            "bmp",
            "png",
            "gif",
            "webp",
            "heic",
            "heif",
            "ai",
            "pict",
            "pic",
            "pct",
            "psd",
            "psb",
            "pdd",
            "tga",
            "tpic",
            "tiff",
            "tif",
        };

        static HashSet<string> SvgExtensions = new HashSet<string>
        {
            "svg",
        };

        public static bool IsImage(string extension)
        {
            return ImageExtensions.Contains(extension?.ToLower());
        }

        public static bool IsSvg(string extension)
        {
            return SvgExtensions.Contains(extension?.ToLower());
        }
    }
}
