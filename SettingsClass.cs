
// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Plugin.Settings.Abstractions.Extensions;

namespace Wordfinder
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string LanguageSettings = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(LanguageSettings, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LanguageSettings, value);
            }
        }


        private const string VolumeFloat = "VolumeFloat_key";
        private static readonly float FloatDefault = (float)1.0;

        public static float Volume
        {
            get
            {
                return AppSettings.GetValueOrDefault(VolumeFloat, FloatDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(VolumeFloat, value);
            }
        }



        private const string RateFloat = "RateFloat_key";
        private static readonly float RateDefault = (float)1.0;

        public static float Rate
        {
            get
            {
                return AppSettings.GetValueOrDefault(RateFloat, RateDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RateFloat, value);
            }
        }


        private const string PitchFloat = "PitchFloat_key";
        private static readonly float PitchDefault = (float)1.0;

        public static float Pitch
        {
            get
            {
                return AppSettings.GetValueOrDefault(PitchFloat, PitchDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PitchFloat, value);
            }
        }

    }

}

      

        


