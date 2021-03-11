using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Timebox.Models
{
    public class VoiceModel
    {
        private readonly SpeechSynthesizer _synthesizer;

        public VoiceModel()
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SetOutputToDefaultAudioDevice();
            _synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
        }

        public VoiceModel(Dictionary<string, string> pronunciationGuide)
        {
            
        }

        public void Speak(string sentence)
        {
            if(sentence==null) return;

            _synthesizer.SpeakAsync(sentence);
        }

        public void SayGreetings()
        {
            string greet = "Good morning";
            int hour = DateTime.Now.Hour;
            if (hour >= 12 && hour < 18)
            {
                greet = "Good evening";
            }
            else if (hour >= 18 && hour < 23)
            {
                greet = "Good night";
            }

            Speak(greet);
        }

        public void SayGoodbye()
        {
            string greet = "Thank you and good bye";

            Speak(greet);
        }

    }
}
