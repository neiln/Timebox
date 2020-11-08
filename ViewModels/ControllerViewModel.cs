using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TimeBox.Models;
using System.Media;
using System.Net.Mime;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TimeBox.Properties;

namespace TimeBox.ViewModels
{
    public class ControllerViewModel : Screen
    {
        private readonly AttendeesViewModel _attendeesViewModel;
        private readonly TunePlayer _tunePlayer;
        private readonly ImageSticker _imageSticker;
        private Button _playButton;
        public event EventHandler<EventArgs> ControllerEvent;
        public ControllerViewModel(AttendeesViewModel attendeesView, TunePlayer tunePlayer, ImageSticker imageSticker)
        {
            _attendeesViewModel = attendeesView;

            AttendeesView = attendeesView;
            _tunePlayer = tunePlayer;
            _imageSticker = imageSticker;
            _tunePlayer.MusicStatusChange += (s, e) =>
            {
                _playButton.Content = _tunePlayer.IsPlaying?"STOP MUSIC":"PLAY MUSIC";
            };

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

        public void ButtonNextQuote()
        {
            _tunePlayer.Stop();
            OnControllerEvent(new DisplayQuotesEventArgs());
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
    }

    public class ChangeNameEventArgs : EventArgs
    {
        public string Name { get; set; }
    }

    public class ResetTimerEventArgs : EventArgs
    {
        public bool IsResetTimer { get; set; }
    }

    public class DisplayQuotesEventArgs : EventArgs
    {
    }

    public class DisplayEmojiEventArgs : EventArgs
    {
        public int EmojiIndex { get; set; }
    }
}
