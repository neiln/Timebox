using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBox.Models
{
    public class AppConfig
    {
        private readonly Configuration _config;
        public AppConfig()
        {

            var exeLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            _config = ConfigurationManager.OpenExeConfiguration(exeLocation);

            AttendeeNames = _config.AppSettings.Settings["Attendees"].Value.ToString().Split(',');
            
            if (int.TryParse(ConfigurationManager.AppSettings["UpdateMinutes"], out int result))
            {
                UpdateMinutes = result;
            }

        }

        public string[] AttendeeNames { get; private set; }
        public int UpdateMinutes { get; private set; } = 3;

        public void Save(string [] attendees, int updateMin)
        {
            AttendeeNames = attendees;
            UpdateMinutes = updateMin;

            _config.AppSettings.Settings["UpdateMinutes"].Value = UpdateMinutes.ToString();
            _config.AppSettings.Settings["Attendees"].Value = string.Join(",", AttendeeNames); ;
            
            _config.Save(ConfigurationSaveMode.Modified);

        }
    }
}
