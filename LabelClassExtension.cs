using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;


using System.Threading;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Wordfinder
{

    public class MyLabel : Label
    {

        public static CrossLocale? locale = null;

        public MyLabel()
        {

            //large font size
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));

            //Bold font
            FontAttributes = FontAttributes.Bold;

            //black colour
            TextColor = Color.Black;

            // adding click to label

            TapGestureRecognizer labelTap = new TapGestureRecognizer();

            GestureRecognizers.Add(labelTap);
            labelTap.Tapped += async delegate
            {

                if (Settings.Language == null)
                {
                    await CrossTextToSpeech.Current.Speak(Text, pitch: Settings.Pitch, volume: Settings.Volume, speakRate: Settings.Rate);
                }

                var locales = await CrossTextToSpeech.Current.GetInstalledLanguages();


                var items = locales.Select(a => a.ToString()).ToArray();

                if (Device.RuntimePlatform == Device.Android)
                    locale = locales.FirstOrDefault(l => l.ToString() == Settings.Language);
                else
                    locale = new CrossLocale { Language = Settings.Language };

                


                await CrossTextToSpeech.Current.Speak(Text, pitch:Settings.Pitch, volume:Settings.Volume, speakRate: Settings.Rate, crossLocale: locale);



            };




        }


    }

}

        // activates text to speech when label clicked
/* public async Task Label_ClickedAsync(object sender, EventArgs e)

        {
            
            var locales = await CrossTextToSpeech.Current.GetInstalledLanguages();


            var items = locales.Select(a => a.ToString()).ToArray();

            if (Device.RuntimePlatform == Device.Android)
                locale = locales.FirstOrDefault(l => l.ToString() == Settings.GeneralSettings);
            else
                locale = new CrossLocale { Language = Settings.GeneralSettings };


            await CrossTextToSpeech.Current.Speak(Text, locale);

         }
        };*/
//}