using System.Globalization;
using System;

namespace EnvironmentVariableExplorer.Services
{
    public class LanguageService
    {
        public event Action OnLanguageChanged;

        public void SetLanguage(string language)
        {
            CultureInfo cultureInfo = new CultureInfo(language == "FR" ? "fr-CA" : "en-US");
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            OnLanguageChanged?.Invoke();
        }
    }
}
