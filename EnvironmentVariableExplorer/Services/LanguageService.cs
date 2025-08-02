using System.Globalization;
using System;

namespace EnvironmentVariableExplorer.Services
{
    public class LanguageService
    {
        public event Action OnLanguageChanged;

        public string CurrentLanguage => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();

        public void SetLanguage(CultureInfo culture)
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Resources.Strings.ResourceManager.ReleaseAllResources();

            OnLanguageChanged?.Invoke();
        }
    }
}
