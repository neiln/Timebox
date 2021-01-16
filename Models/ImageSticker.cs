using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Timebox.Models
{
    /// <summary>
    /// Loads images form Images path
    /// Images must be named from 0 to 15 .png
    /// </summary>
    public class ImageSticker
    {
        public ImageSticker()
        {
            Stickers = new List<string>(){ "🤣", "🤔", "👍", "😎", "💪", "😔", "😉", "🙏", "👌", "👏", "🎉" };

            //string appLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //Debug.Assert(appLocation != null, nameof(appLocation) + " != null");

            //string stickerLocation = Path.Combine(appLocation, "Asset", "Stickers");
            //DirectoryInfo directoryInfo = new DirectoryInfo(stickerLocation);
            //var stickers = directoryInfo.GetFiles("*.png");

            //foreach (FileInfo file in stickers)
            //{
            //    _stickers.Add(file.FullName);
            //}

        }

        public List<string> Stickers { get; }
    }

}
