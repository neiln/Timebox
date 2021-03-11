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

namespace Timebox.Models
{
    public class TunePlayer
    {
        private readonly MediaPlayer _mediaPlayer;
        private readonly IEnumerable<Uri> _soundFiles;
        public event EventHandler<EventArgs> MusicStatusChange;
        public TunePlayer()
        {
            _mediaPlayer = new MediaPlayer();

            string appLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
           // Debug.Assert(appLocation != null, nameof(appLocation) + " != null");

            string mediaFile = Path.Combine(appLocation, @"Asset\Media", "Jeopardy-theme-song.mp3");

            string path = Path.Combine(appLocation, @"Asset\Media");

           _soundFiles = Directory.EnumerateFiles(path).Select(x => new Uri(x)).OrderBy(x => x.AbsolutePath);

            if (File.Exists(mediaFile))
            {
                var mediaUrl = new Uri(mediaFile);

                _mediaPlayer.Open(mediaUrl);
                _mediaPlayer.Volume = 0.5;
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

            var uri = _soundFiles.ElementAt(4);
            _mediaPlayer.Open(uri);
            _mediaPlayer.Play();
            IsPlaying = true;
            OnMusicStatusChangeEvent(EventArgs.Empty);
        }

        public void Play(int idx)
        {
            _mediaPlayer.Stop();

            if (idx < 0) { return; }

            var uri = _soundFiles.ElementAt(idx);
            
            _mediaPlayer.Open(uri);
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
