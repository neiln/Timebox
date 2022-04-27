using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Timebox.Models;

namespace Timebox.ViewModels
{
    public class AttendeesViewModel : Screen
    {
        private readonly Attendees _attendees;
        private string _selectedAttendee;
        private string _selectedBackColor;
        private int _selectedUpdateMin;

        public AttendeesViewModel(Attendees attendees)
        {
            _attendees = attendees;
            UpdateMinutes = new List<int> { 1, 2, 3, 5, 10, 15, 20, 30, 60 };

            _selectedUpdateMin = attendees.UpdateMinutes;
            _selectedBackColor = attendees.BackColor;

        }


        public IObservableCollection<string> Attendees => _attendees.AttendeeList;

        public string SelectedAttendee
        {
            get => _selectedAttendee;
            set
            {
                if (_selectedAttendee != value)
                {
                    _selectedAttendee = value;
                    NotifyOfPropertyChange(nameof(SelectedAttendee));
                }
            }
        }

        public int SelectedUpdateMinute
        {
            get => _selectedUpdateMin;
            set
            {
                if (_selectedUpdateMin != value)
                {
                    _selectedUpdateMin = value;
                    NotifyOfPropertyChange(nameof(SelectedUpdateMinute));

                    SaveChanges();
                }
            }
        }


        public string SelectedBackColor
        {
            get => _selectedBackColor;
            set
            {
                if (_selectedBackColor != value)
                {
                    _selectedBackColor = value;
                    NotifyOfPropertyChange(nameof(SelectedBackColor));

                    SaveChanges();
                }
            }
        }

        public string NewAttendee { get; set; }

        public string GetNextAttendee()
        {
            return _attendees.GetNextAttendee();
        }

        public void AddButton()
        {
            if (string.IsNullOrWhiteSpace(NewAttendee)) return;

            if (_attendees.AttendeeList.FirstOrDefault(x=>x.ToLower()==NewAttendee.ToLower())!=null)
            {
                return;
            }

            _attendees.AttendeeList.Add(NewAttendee.Trim());

            SaveChanges();
            NewAttendee = "";
            NotifyOfPropertyChange(nameof(NewAttendee));
        }

        public List<int> UpdateMinutes { get; set; }

        public void RemoveButton()
        {
            if (string.IsNullOrWhiteSpace(SelectedAttendee)) return;

            _attendees.AttendeeList.Remove(SelectedAttendee);
            SaveChanges();

            Attendees.Remove(SelectedAttendee);

        }

        public void SkipButton()
        {

        }

        public void ShuffleButton()
        {
            _attendees.ShuffleAttendeeList();
        }

        private void SaveChanges()
        {
            _attendees.Save(_attendees.AttendeeList.ToArray(), SelectedUpdateMinute, SelectedBackColor);
        }
    }
}
