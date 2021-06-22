// Developer: Neil Nelson
// Developed for scrum teams to provide time box update
//
using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
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
        private Label _displayTextControl;
        private readonly VoiceModel _voiceModel;
        private Brush _backBrush = new SolidColorBrush(Colors.Green);
        public string AppTitle => "Scrum Time!";

        public ShellViewModel(IWindowManager windowManager, 
                              ClockViewModel clockView, 
                              QuotesViewModel quotesView, 
                              ImagesViewModel imagesView,
                              VoiceModel voiceModel,
                              ControllerViewModel controllerView)
        {
            ClockView = clockView;
            QuotesView = quotesView;
            ImagesView = imagesView;
            _windowManager = windowManager;
            _controllerView = controllerView;
            _voiceModel = voiceModel;

            _controllerView.ControllerEvent += async (sender, eArg) =>
            {
                if (eArg is ChangeNameEventArgs nextAttendee)
                {
                    await ClockView.Stop();

                    if (DisplayTitle != "GOOD MORNING!" && DisplayTitle != "THANK YOU!")
                    {
                        //_voiceModel.Speak("Next");
                    }

                    _displayText = "------";
                    NotifyOfPropertyChange(nameof(DisplayTitle));

                    await Task.Run(() =>
                    {
                        Thread.Sleep(1000);
                    });


                    //string speechText = PronounceText(nextAttendee.Name);
                    //_voiceModel.Speak(speechText);

                    DisplayTitle = nextAttendee.Name.ToUpper();

                    await ClockView.Start();

                    NotifyOfPropertyChange(nameof(DisplayTitle));
                }

                if (eArg is ResetTimerEventArgs reset)
                {
                    if (reset.IsResetTimer)
                    {
                        DisplayTitle = "THANK YOU!";
                        await ClockView.Stop();
                        
                        NotifyOfPropertyChange(nameof(DisplayTitle));

                        // if (_controllerView.TextBlockQuote != null)
                        // {
                        //     voiceModel.Speak("Thanks, That's all folks, let's do a trivia question");
                        //     await Task.Run(() => { Thread.Sleep(5000); });
                        //
                        //     _controllerView.PlayWaitMusic();
                        //     ShowQuotes(_controllerView.TextBlockQuote);
                        // }
                    }
                }

                if (eArg is DisplayQuotesEventArgs quotes)
                {
                    ShowQuotes(quotes.Text);
                }

                if (eArg is DisplayEmojiEventArgs displayEmojiEvent)
                {
                    ShowEmoji(displayEmojiEvent.EmojiIndex);
                }

                if (eArg is ChangeBackColorEventArgs changeColorEvent)
                {
                   BackBrush = new SolidColorBrush(changeColorEvent.BackColor);
                }
            };
            
            SetGreeting();
        }

        private void SetGreeting()
        {
            if (DateTime.Now.Hour > 6 && DateTime.Now.Hour < 11)
            {
                _displayText = "GOOD MORNING!";
            }
            else if (DateTime.Now.Hour > 11 && DateTime.Now.Hour < 14)
            {
                _displayText = "GOOD AFTERNOON!";
            }
            else
            {
                _displayText = "GOOD EVENING!";
            }
        }

        private void ShowQuotes(string text)
        {
            _clockSideControl.Visibility = Visibility.Collapsed;
            _imageSideControl.Visibility = Visibility.Collapsed;
            _quoteSideControl.Visibility = Visibility.Visible;
            QuotesView.SetText(text);

            _voiceModel.Speak(text);

        }

        private void ShowEmoji(int idx)
        {
            _quoteSideControl.Visibility = Visibility.Collapsed;
            _clockSideControl.Visibility = Visibility.Visible;
            _imageSideControl.Visibility = Visibility.Collapsed;

            ImagesView.ShowImage(idx);

            _imageSideControl.Visibility = Visibility.Visible;
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


        public void DisplayTextLoaded(Label ctrl)
        {
            _displayTextControl = ctrl;
        }

        private void ShowController()
        {

            dynamic settings = new ExpandoObject();
            settings.WindowStyle = WindowStyle.ThreeDBorderWindow;
            settings.ShowInTaskbar = false;
            settings.Title = "Controller";
            settings.WindowStyle = WindowStyle.SingleBorderWindow;
            settings.WindowState = WindowState.Normal;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.Top = 120;
            settings.Left = 1100;

            _windowManager.ShowWindow(_controllerView, null, settings);

        }

    }

}
