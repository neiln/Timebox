﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timebox.Models
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

            BackColor = ConfigurationManager.AppSettings["BackColor"];

        }

        public string[] AttendeeNames { get; private set; }
        public int UpdateMinutes { get; private set; } = 3;

        public string BackColor { get; set; } = "Green";

        public void Save(string [] attendees, int updateMin, string backColor)
        {
            AttendeeNames = attendees;
            UpdateMinutes = updateMin;

            _config.AppSettings.Settings["UpdateMinutes"].Value = updateMin.ToString();
            _config.AppSettings.Settings["Attendees"].Value = string.Join(",", attendees); ;
            _config.AppSettings.Settings["BackColor"].Value = backColor;

            _config.Save(ConfigurationSaveMode.Modified);

        }
    }
}
