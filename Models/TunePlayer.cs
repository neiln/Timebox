using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TimeBox.Models
{
    public class TunePlayer
    {
        private readonly MediaPlayer _mediaPlayer;
        public event EventHandler<EventArgs> MusicStatusChange;
        public TunePlayer()
        {
            _mediaPlayer = new MediaPlayer();

            string appLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Debug.Assert(appLocation != null, nameof(appLocation) + " != null");

            string mediaFile = Path.Combine(appLocation, "Asset", "Jeopardy-theme-song.mp3");

            if (File.Exists(mediaFile))
            {
                var mediaUrl = new Uri(mediaFile);

                _mediaPlayer.Open(mediaUrl);
                _mediaPlayer.Volume = 0.2;
            }

            _mediaPlayer.MediaEnded += (s, e) =>
            {
                IsPlaying = false;
                OnMusicStatusChangeEvent(EventArgs.Empty);
            };
        }

        public bool IsPlaying { get; set; } = false;

        public void Play()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Play();
            IsPlaying = true;
            OnMusicStatusChangeEvent(EventArgs.Empty);
        }

        public void Stop()
        {
            _mediaPlayer.Stop();
            IsPlaying = false;
            OnMusicStatusChangeEvent(EventArgs.Empty);
        }
        protected virtual void OnMusicStatusChangeEvent(EventArgs e)
        {
            EventHandler<EventArgs> handler = MusicStatusChange;
            handler?.Invoke(this, e);
        }

    }

}
