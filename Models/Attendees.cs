using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Syroot.Windows.IO;

namespace Timebox.Models
{
    public class Attendees
    {
        private readonly AppConfig _appConfig;
        private int _currentSelectionIdx;

        public Attendees(AppConfig appConfig)
        {
            AttendeeList = new BindableCollection<string>();
            _appConfig = appConfig;

            ShuffleAttendeeList();
        }

        public void ShuffleAttendeeList()
        {
            _currentSelectionIdx = 0;
            var names = ShuffleNames(_appConfig.AttendeeNames);
            AttendeeList.Clear();
            AttendeeList.AddRange(names);

        }

        public IObservableCollection<string> AttendeeList { get; set; }
        public string GetNextAttendee()
        {
            if (_currentSelectionIdx > AttendeeList.Count - 1)
            {
                ShuffleAttendeeList();
                return string.Empty;
            }

            string name = AttendeeList[_currentSelectionIdx];
            _currentSelectionIdx++;

            return name;
        }

        public int UpdateMinutes => _appConfig.UpdateMinutes;

        public string BackColor => _appConfig.BackColor;

        public void Save(string[] attendees, int updateMin, string backColor)
        {
            _appConfig.Save(attendees, updateMin, backColor);
        }

        private string[] ShuffleNames(string[] attendees)
        {
            Random random = new Random();
            var result = _appConfig.AttendeeNames.Where(x => x.Length > 0).OrderBy(x => random.Next()).Select(x=>x.ToUpper()).ToArray();

            return result;
        }
    }
}
