using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Timebox.Models;

namespace Timebox.ViewModels
{
    public class QuotesViewModel : Screen
    {
        public string Text { get; set; }

        public void SetText(string text)
        {
            Text = text;
            NotifyOfPropertyChange(nameof(Text));
        }


    }
}
