using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Timebox.Models;
using System.Media;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Timebox.Properties;
using Color = System.Windows.Media.Color;

namespace Timebox.ViewModels
{
    public class ControllerViewModel : Screen
    {
        private readonly AttendeesViewModel _attendeesViewModel;
        private readonly TunePlayer _tunePlayer;
        private readonly ImageSticker _imageSticker;
        private readonly ChuckApi _chuckApi;
        private Button _playButton;
        public event EventHandler<EventArgs> ControllerEvent;
        public ControllerViewModel(AttendeesViewModel attendeesView, TunePlayer tunePlayer, ImageSticker imageSticker, ChuckApi chuckApi)
        {
            _attendeesViewModel = attendeesView;

            AttendeesView = attendeesView;
            _tunePlayer = tunePlayer;
            _imageSticker = imageSticker;
            _chuckApi = chuckApi;
            _tunePlayer.MusicStatusChange += (s, e) =>
            {
                _playButton.Content = _tunePlayer.IsPlaying ? "STOP MUSIC" : "PLAY MUSIC";
            };

        }

        protected override void OnActivate()
        {
            SelectedBackgroundColor = _attendeesViewModel.SelectedBackColor;
        }

        protected override void OnDeactivate(bool close)
        {
            //Save Background Color
            _attendeesViewModel.SelectedBackColor = _selectedBackgroundColor;
            base.OnDeactivate(close);
        }

        public AttendeesViewModel AttendeesView { get; }

        public void WrapCanvas(WrapPanel panel)
        {
            foreach (var sticker in _imageSticker.Stickers)
            {
                int idx = _imageSticker.Stickers.IndexOf(sticker);
                panel.Children.Add(GetButton(idx, sticker));
            }

            Button btnClr = GetButton(-1, null);
            btnClr.Content = "X";
            panel.Children.Add(btnClr);
        }

        private Button GetButton(int idx, string sticker)
        {
            Button btn = new Button
            {
                Tag = idx,
                Content = sticker,//!=null ? new Image() { Source = new BitmapImage(new Uri(sticker))} : null,
                Width = 60,
                Height = 60,
                FontSize = 32,
                Margin = new Thickness(2),
                Background = new SolidColorBrush(Colors.White),
            };
            btn.Click += (s, e) => { OnControllerEvent(new DisplayEmojiEventArgs() { EmojiIndex = idx }); };

            return btn;
        }

        public BindableCollection<string> BackgroundColors
        {
            get
            {
                return new BindableCollection<string>(new []{ "Green", "White Smoke", "Royal Blue", "Dark Sea Green", "Orange Red", "Aqua", "Alice Blue" });
            }
        }

        private string _selectedBackgroundColor;
        public string SelectedBackgroundColor
        {
            get => _selectedBackgroundColor;
            set
            {
                _selectedBackgroundColor = value;
                NotifyOfPropertyChange(nameof(SelectedBackgroundColor));

                Color bkColor = Colors.Green;

                if (_selectedBackgroundColor == "White Smoke") bkColor = Colors.WhiteSmoke;
                if (_selectedBackgroundColor == "Royal Blue") bkColor = Colors.RoyalBlue;
                if (_selectedBackgroundColor == "Dark Sea Green") bkColor = Colors.DarkSeaGreen;
                if (_selectedBackgroundColor == "Orange Red") bkColor = Colors.OrangeRed;
                if (_selectedBackgroundColor == "Aqua") bkColor = Colors.Aqua;
                if (_selectedBackgroundColor == "Alice Blue") bkColor = Colors.AliceBlue;

                OnControllerEvent(new ChangeBackColorEventArgs() { BackColor = bkColor });

            }
        }

        public void ButtonNextName(Button sender)
        {
            
            _tunePlayer.Stop();

            var attendeeName = _attendeesViewModel.GetNextAttendee();

            var reset = (attendeeName.Length == 0);

            if (reset)
            {
                sender.Content = "START";
                OnControllerEvent(new ResetTimerEventArgs() { IsResetTimer = true });
            }
            else
            {
                sender.Content = "NEXT >>";
                OnControllerEvent(new ChangeNameEventArgs() { Name = attendeeName });
            }
        }

        public string TextBlockQuote { get; set; }
        public void ButtonNextQuote()
        {
            _tunePlayer.Stop();
            GetChuckFact();
        }

        public void ButtonShowText()
        {
            _tunePlayer.Stop();
            OnControllerEvent(new DisplayQuotesEventArgs() { Text = TextBlockQuote });
        }

        public void ButtonPlayMusicLoaded(Button button)
        {
            _playButton = button;

            _playButton.Click += (s, e) =>
            {
                ButtonPlayMusic(_playButton);
            };
        }

        public void ButtonPlayMusic(Button button)
        {

            if (_tunePlayer.IsPlaying)
            {
                _tunePlayer.Stop();
                button.Content = "PLAY MUSIC";
            }
            else
            {
                _tunePlayer.Play();
                button.Content = "STOP MUSIC";
            }
        }

        public void ButtonShowEmoticon(int id)
        {
            OnControllerEvent(new DisplayEmojiEventArgs() { EmojiIndex = id });
        }

        protected virtual void OnControllerEvent(EventArgs e)
        {
            EventHandler<EventArgs> handler = ControllerEvent;
            handler?.Invoke(this, e);
        }


        private async void GetChuckFact()
        {
            try
            {
                var result = await _chuckApi.GetChuckFact();
                TextBlockQuote = !string.IsNullOrWhiteSpace(result.ResponseReason)
                    ? result.ResponseReason
                    : $"“{result.Value}”";
            }
            catch (Exception ex)
            {
                TextBlockQuote = ex.Message;
            }
            finally
            {
                NotifyOfPropertyChange(nameof(TextBlockQuote));
            }

        }
    }

}
