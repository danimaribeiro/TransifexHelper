using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransifexApi.Base
{
    public class Configuration
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ActiveProject { get; set; }

        public string LanguadeCode { get; set; }

        public int TimeBetweenTranslation { get; set; }

        public static Configuration GetDefault()
        {
            var config = new Configuration();
            config.ActiveProject = TransifexApi.Properties.Settings.Default.ActiveProject;
            config.Username = TransifexApi.Properties.Settings.Default.Username;
            config.Password = TransifexApi.Properties.Settings.Default.Password;
            config.LanguadeCode = TransifexApi.Properties.Settings.Default.LanguageCode;
            config.TimeBetweenTranslation = TransifexApi.Properties.Settings.Default.TimeBetweenNotification;
            if (config.IsValid())
                return config;
            else
                return null;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(this.ActiveProject))
                return false;
            if (string.IsNullOrWhiteSpace(this.Username))
                return false;
            if (string.IsNullOrWhiteSpace(this.Password))
                return false;
            if (string.IsNullOrWhiteSpace(this.LanguadeCode))
                return false;
            if (this.TimeBetweenTranslation <= 0)
                return false;
            return true;
        }
    }
}
