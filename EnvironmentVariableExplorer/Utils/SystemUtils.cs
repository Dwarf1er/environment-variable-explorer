using System;
using System.Globalization;

namespace EnvironmentVariableExplorer.Utils
{
    public static class SystemUtils
    {
        public static readonly bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public static string GetSystemLanguage()
        {
            var systemLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper();
            return (systemLanguage == "FR" || systemLanguage == "EN") ? systemLanguage : "EN";
        }
    }
}
