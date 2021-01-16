using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Timebox.ViewModels
{
    public sealed class ChangeNameEventArgs : EventArgs
    {
        public string Name { get; set; }
    }

    public sealed class ResetTimerEventArgs : EventArgs
    {
        public bool IsResetTimer { get; set; }
    }

    public sealed class DisplayQuotesEventArgs : EventArgs
    {
        public string Text { get; set; }
    }

    public sealed class DisplayEmojiEventArgs : EventArgs
    {
        public int EmojiIndex { get; set; }
    }

    public sealed class ChangeBackColorEventArgs : EventArgs
    {
        public Color BackColor { get; set; }
    }
}
