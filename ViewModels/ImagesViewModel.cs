using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Timebox.Models;

namespace Timebox.ViewModels
{
    public class ImagesViewModel : Screen
    {
        private readonly ImageSticker _imageSticker;

        public ImagesViewModel(ImageSticker imageSticker)
        {
            _imageSticker = imageSticker;
        }

        public void ShowImage(int idx)
        {
            if (idx == -1 || idx == 13)
            {
                StickerImage = null;
                NotifyOfPropertyChange(nameof(StickerImage));
                return;
            }

            if (idx < _imageSticker.Stickers.Count)
            {
                StickerImage = _imageSticker.Stickers[idx];
                NotifyOfPropertyChange(nameof(StickerImage));
            }
        }
        public string StickerImage { get; set; }

    }
}
