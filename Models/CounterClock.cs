using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Timebox.ViewModels;

namespace Timebox.Models
{
    public class CounterClock
    {
        private readonly Action _timesUp;
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly IProgress<int> _timeProgress;

        private int _totalSeconds = 10;
        private int _currentSeconds = 1;

        private bool _isReset = false;
        public CounterClock(Action<int> updateTimeDisplayMethod,
            Action timesUpMethod)
        {
            _timesUp = timesUpMethod;
            _dispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            _dispatcherTimer.Tick += dispatcherTimer_Tick;

            _timeProgress = new Progress<int>(updateTimeDisplayMethod);
        }

        public void SetUpdateMinutes(int minutes)
        {
            if (minutes > 60) minutes = 60;
            _totalSeconds = minutes * 60;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (_isReset)
            {
                _currentSeconds = _totalSeconds;
                _isReset = false;
            }

            if (_currentSeconds == -1)
            {
                _timesUp();
            }

            _timeProgress.Report(_currentSeconds);
            _currentSeconds -= 1;

        }

        public async Task Reset()
        {
            await Task.Run(() =>
            {
                _dispatcherTimer.Stop();
                _isReset = true;
                _dispatcherTimer.Start();

            });
        }

        public async Task Stop()
        {
            await Task.Run(() =>
            {
                _timeProgress.Report(0);
                _dispatcherTimer.Stop();
            });
        }

        public async Task Pause()
        {
            await Task.Run(() =>
            {
                _dispatcherTimer.Stop();
            });
        }

        public async Task Resume()
        {
            await Task.Run(() =>
            {
                _dispatcherTimer.Start();
            });
        }
    }
}
