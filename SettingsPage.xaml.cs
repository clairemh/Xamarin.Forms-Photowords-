using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wordfinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public static CrossLocale? locale = null;



        public SettingsPage()
        {


            InitializeComponent();

              LanguageLabel.Text = "Language: " + Settings.Language;

                 PitchSlider.Value = Settings.Pitch;
                 VolumeSlider.Value = Settings.Volume;
                 SpeedSlider.Value = Settings.Rate;
                 



            //function declaration for bottom banner clicked
            BottomBanner.Clicked += BottomBanner_Clicked;



            // gets available languages
               Language.Clicked += async (sender, args) =>

                    {
                        var locales = await CrossTextToSpeech.Current.GetInstalledLanguages();


                        var items = locales.Select(a => a.ToString()).ToArray();


                        int count = 0;

                        // counting the available English languages on device
                        for (int i = 0; i < items.Length; i++)
                        {
                            if (items[i].Substring(0, 2) == "en" || items[i].Substring(0, 2) == "En")
                            {
                                count++;
                            }

                        }


                        // adding the available English languages to new array up to language count
                        string[] itemsarray = new string[count];

                        int j = 0;
                        int k = 0;
                        for (k = 0; k < items.Length; k++)
                        {
                            if (items[k].Substring(0, 2) == "en" || items[k].Substring(0, 2) == "en")
                            {

                                itemsarray[j] = items[k];
                                j++;
                            }

                            if (j == count)
                                break;
                        }


                        // displaying the available English languages in new array on action sheet

                        var selected = await DisplayActionSheet("Language", "OK", null, itemsarray);

                        if (string.IsNullOrWhiteSpace(selected) || selected == "OK")
                            return;

                        LanguageLabel.Text = "Language: " + selected;

                        if (Device.RuntimePlatform == Device.Android)
                            locale = locales.FirstOrDefault(l => l.ToString() == selected);
                        else
                            locale = new CrossLocale { Language = selected };

                        //saving in Settings
                        Settings.Language = selected;

                        //calling speak method with current settings
                        await CrossTextToSpeech.Current.Speak("Language updated", pitch: Settings.Pitch, volume: Settings.Volume, speakRate: Settings.Rate, crossLocale: locale);





                    };
              }




        


        //return to home page if bottom button clicked
        private async void BottomBanner_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

        void OnSpeedValueChanged(object sender, ValueChangedEventArgs args)
        {

            //setting variable to save speed from value on slider
            double speed = SpeedSlider.Value;

            float speedfloat = (float)speed;

            // saving to settings
            Settings.Rate = speedfloat;


        }

        void OnVolumeValueChanged(object sender, ValueChangedEventArgs args)
        {

            //saving volume as variable depending slider value
            double volume = VolumeSlider.Value;

            float volumefloat = (float)volume;

            //saving to settings
            Settings.Volume = volumefloat;
            


        }

        void OnPitchValueChanged(object sender, ValueChangedEventArgs args)
        {


            double pitch = PitchSlider.Value;


            float pitchfloat = (float)pitch;

         Settings.Pitch = pitchfloat;


        }

    }
}

            