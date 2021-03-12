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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using Timebox.Models;

namespace Timebox.ViewModels
{
    public class ClockViewModel : Screen
    {
        private readonly AppConfig _appConfig;
        private readonly TunePlayer _tunePlayer;
        private readonly CounterClock _counterClock;
        public ClockViewModel(AppConfig appConfig, TunePlayer tunePlayer)
        {
            _appConfig = appConfig;
            _tunePlayer = tunePlayer;

            _counterClock = new CounterClock(UpdateDisplayTime, TimesUp);
            int totalUpdateMinutes = _appConfig.UpdateMinutes;
            _counterClock.SetUpdateMinutes(totalUpdateMinutes);

           
        }

        protected override void OnViewLoaded(object view)
        {
            Time = "00:00";
        }

        private void TimesUp()
        {
            _tunePlayer.Play(8);
        }

        public string Date { get; } = DateTime.Today.ToString("ddd, dd MMM", CultureInfo.CreateSpecificCulture("en-US")).ToUpper();

        public string Time
        {
            get => "00:00";
            set
            {
                if (_flashSeconds)
                {
                    _timeGridCtrl.Name = "Start";
                }
               
                var time = value;
                MinLabel1 = time[0].ToString();
                MinLabel2 = time[1].ToString();
                SecLabel1 = time[3].ToString();
                SecLabel2 = time[4].ToString();

                if (_flashSeconds)
                {
                    _timeGridCtrl.Name = "Stop";
                }


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

        private bool _flashSeconds = false;
        public void UpdateDisplayTime(int seconds)
        {
            if (seconds!=0 && seconds < 10)
            {
                _flashSeconds = true;
            }
            else
            {
                _flashSeconds = false;
            }

            TimeSpan ts = new TimeSpan(0, 0, seconds);
            Time = ts.ToString(@"mm\:ss");

            NotifyOfPropertyChange(nameof(Time));
        }

        public async Task Stop()
        {
            _timeGridCtrl.Name = "None";
            await _counterClock.Stop();
        }

        public async Task Start()
        {
            int totalUpdateMinutes = _appConfig.UpdateMinutes;
            _counterClock.SetUpdateMinutes(totalUpdateMinutes);
            await _counterClock.Reset();
        }

        private Grid _timeGridCtrl;
        public void TimeGridLoaded(Grid ctrl)
        {
            _timeGridCtrl = ctrl;
        }
    }

}
