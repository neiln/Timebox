using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timebox.Models
{
    public class AppConfig
    {
        private readonly string _configPath;
        //private readonly Configuration _config;
        public AppConfig()
        {

            _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),"Timebox");
      
            if (!Directory.Exists(_configPath))
            {
                Directory.CreateDirectory(_configPath);
            }
            else
            {
                string file = Path.Combine(_configPath, "Attendees.txt");
                var content = File.ReadAllLines(file);
                AttendeeNames = content[0].Remove(0, content[0].IndexOf('=')+1).Split(',');

                if (int.TryParse(content[1].Remove(0, content[1].IndexOf('=')+1), out int result))
                {
                    UpdateMinutes = result;
                }

                BackColor = content[2].Remove(0, content[2].IndexOf('=')+1);
            }


            //var exeLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            //_config = ConfigurationManager.OpenExeConfiguration(exeLocation);

            //AttendeeNames = _config.AppSettings.Settings["Attendees"].Value.ToString().Split(',');
            
            //if (int.TryParse(ConfigurationManager.AppSettings["UpdateMinutes"], out int result))
            //{
            //    UpdateMinutes = result;
            //}

            //BackColor = ConfigurationManager.AppSettings["BackColor"];

        }

        public string[] AttendeeNames { get; private set; }
        public int UpdateMinutes { get; private set; } = 3;

        public string BackColor { get; set; } = "Green";

        public void Save(string [] attendees, int updateMin, string backColor)
        {
            AttendeeNames = attendees;
            UpdateMinutes = updateMin;

            //_config.AppSettings.Settings["UpdateMinutes"].Value = updateMin.ToString();
            //_config.AppSettings.Settings["Attendees"].Value = string.Join(",", attendees); ;
            //_config.AppSettings.Settings["BackColor"].Value = backColor;

            //_config.Save(ConfigurationSaveMode.Modified);


            string[] lines =
            {
                $"Attendees={string.Join(",", attendees)}",
                $"UpdateMinutes={updateMin}",
                $"BackColor={backColor}"
            };

            string file= Path.Combine(_configPath, "Attendees.txt");
            File.WriteAllLines(file, lines);
        }

    }
}
