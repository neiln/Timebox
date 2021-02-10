// Developer: Neil Nelson
// Developed for scrum teams to provide time box update
//
using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using System.Windows.Media;
using Timebox.Models;
using Timebox.Views;

namespace Timebox.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly ControllerViewModel _controllerView;
        private Brush _backBrush = new SolidColorBrush(Colors.Green);
        public string AppTitle => "Scrum Time!";

        public ShellViewModel(IWindowManager windowManager, 
                              ClockViewModel clockView, 
                              QuotesViewModel quotesView, 
                              ImagesViewModel imagesView,
                              ControllerViewModel controllerView)
        {
            

            ClockView = clockView;
            QuotesView = quotesView;
            ImagesView = imagesView;

            _windowManager = windowManager;
            _controllerView = controllerView;
           
            _controllerView.ControllerEvent += async (sender, eArg) =>
            {
                if (eArg is ChangeNameEventArgs change)
                {
                    await ClockView.Stop();

                    _displayText = "????";
                    NotifyOfPropertyChange(nameof(DisplayTitle));

                    await Task.Run(() =>
                    {
                        Thread.Sleep(2000);
                    });


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
                    _clockSideControl.Visibility = Visibility.Collapsed;
                    _imageSideControl.Visibility = Visibility.Collapsed;
                    _quoteSideControl.Visibility = Visibility.Visible;
                    QuotesView.SetText(quotes.Text);
                    
                }

                if (eArg is DisplayEmojiEventArgs displayEmojiEvent)
                {
                    _quoteSideControl.Visibility = Visibility.Collapsed;
                    _clockSideControl.Visibility = Visibility.Visible;
                    _imageSideControl.Visibility = Visibility.Collapsed;
                    
                    ImagesView.ShowImage(displayEmojiEvent.EmojiIndex);

                    _imageSideControl.Visibility = Visibility.Visible;
                }

                if (eArg is ChangeBackColorEventArgs changeColorEvent)
                {
                   BackBrush = new SolidColorBrush(changeColorEvent.BackColor);
                }
            };


        }


        public Brush BackBrush
        {
            get => _backBrush;
            set
            {
                _backBrush = value; 
                NotifyOfPropertyChange(nameof(BackBrush));
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            ShowController();
        }

        public ClockViewModel ClockView { get; }

        public QuotesViewModel QuotesView { get; }

        public ImagesViewModel ImagesView { get; }

        private string _displayText = "GOOD MORNING!";
        public string DisplayTitle
        {
            get => _displayText;
            set
            {
                _displayTextControl.Visibility = Visibility.Collapsed;
                _displayText = value;
                _displayTextControl.Visibility = Visibility.Visible;
            }
        }

        private StackPanel _clockSideControl;
        public void ClockSideLoaded(StackPanel ctrl)
        {
            _clockSideControl = ctrl;
        }

        private ContentControl _quoteSideControl;
        public void QuoteSideLoaded(ContentControl ctrl)
        {
            _quoteSideControl = ctrl;
        }

        private ContentControl _imageSideControl;
        public void ImageSideLoaded(ContentControl ctrl)
        {
            _imageSideControl = ctrl;
        }

        private Label _displayTextControl;
        public void DisplayTextLoaded(Label ctrl)
        {
            _displayTextControl = ctrl;
        }

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
