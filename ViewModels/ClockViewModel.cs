using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using TimeBox.Models;

namespace TimeBox.ViewModels
{
    public class ClockViewModel : Screen
    {
        private readonly TunePlayer _tunePlayer;
        private readonly CounterClock _counterClock;
        public ClockViewModel(AppConfig appConfig, TunePlayer tunePlayer)
        {
            int totalUpdateMinutes = appConfig.UpdateMinutes;
            _tunePlayer = tunePlayer;
            _counterClock = new CounterClock(UpdateDisplayTime, TimesUp);
            _counterClock.SetUpdateMinutes(totalUpdateMinutes);

            Time = "00:00";
        }

        private void TimesUp()
        {
            _tunePlayer.Play();
        }

        public string Date { get; } = DateTime.Today.ToString("ddd, dd MMM yy", CultureInfo.CreateSpecificCulture("en-US"));

        public string Time
        {
            get => "00:00";
            set
            {
                var time = value;
                MinLabel1 = time[0].ToString();
                MinLabel2 = time[1].ToString();
                SecLabel1 = time[3].ToString();
                SecLabel2 = time[4].ToString();
                NotifyOfPropertyChange(nameof(MinLabel1));
                NotifyOfPropertyChange(nameof(MinLabel2));
                NotifyOfPropertyChange(nameof(SecLabel1));
                NotifyOfPropertyChange(nameof(SecLabel2));
            }
        }

        public string MinLabel1 { get; set; }
        public string MinLabel2 { get; set; }
        public string SecLabel1 { get; set; }
        public string SecLabel2 { get; set; }

        public void UpdateDisplayTime(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, seconds);
            Time = ts.ToString(@"mm\:ss");

            NotifyOfPropertyChange(nameof(Time));
        }

        public async Task Stop()
        {
            await _counterClock.Stop();
        }

        public async Task Start()
        {
            await _counterClock.Reset();
        }

    }

}
