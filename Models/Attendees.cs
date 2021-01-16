using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Syroot.Windows.IO;

namespace Timebox.Models
{
    public class Attendees
    {
        private readonly AppConfig _appConfig;
        public List<string> AttendeeList;
        
        public Attendees(AppConfig appConfig)
        {
            AttendeeList = new List<string>();
            _appConfig = appConfig;
            AttendeeList.AddRange(_appConfig.AttendeeNames);
        }

        public string GetNextAttendee()
        {

            if (AttendeeList.Count == 0)
            {
                AttendeeList.Clear();

                if (_appConfig.AttendeeNames != null && _appConfig.AttendeeNames.Length > 0)
                {
                    AttendeeList.AddRange(_appConfig.AttendeeNames);
                }

                return string.Empty;
            }

            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int idx = rand.Next(AttendeeList.Count);
            string name = AttendeeList[idx].ToString();
            AttendeeList.RemoveAt(idx);

            return name;
        }

        public int UpdateMinutes => _appConfig.UpdateMinutes;

        public string BackColor => _appConfig.BackColor;

        public void Save(string[] attendees, int updateMin, string backColor)
        {
            _appConfig.Save(attendees, updateMin, backColor);
        }

    }
}
