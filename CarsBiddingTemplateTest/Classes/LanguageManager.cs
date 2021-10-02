using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace CarsBiddingUsingBootstrap.Classes
{
    public class LanguageManager
    {
        public static List<Language> GetAvailableLanguages() 
        {
            return new List<Language>()
            {
                new Language {
                LanguageFullName = Localization.English, LanguageCultureName = "en"
                },

                new Language {
                    LanguageFullName = Localization.Arabic, LanguageCultureName = "ar"
                }
            };
        }
        public static bool IsLanguageAvailable(string lang)
        {
            return GetAvailableLanguages().Where(a => a.LanguageCultureName.Equals(lang)).FirstOrDefault() != null ? true : false;
        }
        public static string GetDefaultLanguage()
        {
            return GetAvailableLanguages().FirstOrDefault().LanguageCultureName;
        }
        public static void SetLanguage(string lang)
        {
            try
            {
                if (!IsLanguageAvailable(lang)) lang = GetDefaultLanguage();
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            }
            catch (Exception ex) 
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "SetLanguage function,LanguageManager Class");
            }
        }
    }
    public class Language 
    {
        public string LanguageFullName { get; set; }
        public string LanguageCultureName { get; set; }
    }
}