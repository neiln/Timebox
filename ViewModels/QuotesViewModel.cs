using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using TimeBox.Models;

namespace TimeBox.ViewModels
{
    public class QuotesViewModel : Screen
    {
        private readonly ChuckApi _chuckApi;
        private readonly ImageSticker _imageSticker;

        public QuotesViewModel(ChuckApi chuckApi, ImageSticker imageSticker)
        {
            _chuckApi = chuckApi;
            _imageSticker = imageSticker;
        }

        public string Text { get; set; }

        public void RefreshQuote()
        {
            GetChuckFact();
        }

        public void ShowImage(int idx)
        {
            if (idx == -1)
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

        private async void GetChuckFact()
        {
            try
            {
                var result = await _chuckApi.GetChuckFact();
                Text = !string.IsNullOrWhiteSpace(result.ResponseReason)
                    ? result.ResponseReason
                    : $"“{result.Value}”";
            }
            catch (Exception ex)
            {
                Text = ex.Message;
            }
            finally
            {
                NotifyOfPropertyChange(nameof(Text));
            }

        }
    }
}
