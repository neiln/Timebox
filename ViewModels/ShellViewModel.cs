// Developer: Neil Nelson
// Developed for scrum teams to provide time box update
//
using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using System.Windows.Media;
using TimeBox.Models;
using TimeBox.Views;

namespace TimeBox.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly ControllerViewModel _controllerView;
        public string AppTitle => "Scrum Time!";
        public Brush ChromaKeyColor => new SolidColorBrush(Colors.Green);

        public ShellViewModel(IWindowManager windowManager, 
                              ClockViewModel clockView, 
                              QuotesViewModel quotesView, 
                              ControllerViewModel controllerView)
        {
            
            DisplayTitle = "GOOD MORNING!";

            ClockView = clockView;
            QuotesView = quotesView;

            _windowManager = windowManager;
            _controllerView = controllerView;

            _controllerView.ControllerEvent += async (sender, eArg) =>
            {
                if (eArg is ChangeNameEventArgs change)
                {
                    await ClockView.Stop();
                    DisplayTitle = change.Name.ToUpper();
                    await ClockView.Start();
                    
                    NotifyOfPropertyChange(nameof(DisplayTitle));
                }

                if (eArg is ResetTimerEventArgs reset)
                {
                    if (reset.IsResetTimer)
                    {
                        DisplayTitle = "THANK-YOU!";
                        await ClockView.Stop();
                        
                        NotifyOfPropertyChange(nameof(DisplayTitle));
                    }
                }

                if (eArg is DisplayQuotesEventArgs quotes)
                {
                    QuotesView.RefreshQuote();
                }

                if (eArg is DisplayEmojiEventArgs e)
                {
                    //QuotesView.Text = "\"SOMETHING BETTER THAN NOTHING\"";
                    //QuotesView.NotifyOfPropertyChange(nameof(QuotesView.Text));
                    QuotesView.ShowImage(e.EmojiIndex);
                }
            };


        }

        protected override void OnActivate()
        {
            base.OnActivate();

            ShowController();
        }

        public ClockViewModel ClockView { get; }

        public QuotesViewModel QuotesView { get; }

        public string DisplayTitle { get; set; }

        private void ShowController()
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStyle = WindowStyle.ThreeDBorderWindow;
            settings.ShowInTaskbar = true;
            settings.Title = "Controller";
            settings.WindowState = WindowState.Normal;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.Top = 120;
            settings.Left = 1100;

            _windowManager.ShowWindow(_controllerView, null, settings);
        }

    }
}
