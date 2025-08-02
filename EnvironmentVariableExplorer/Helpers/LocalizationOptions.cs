using System.Collections.Generic;
using System.Globalization;

namespace EnvironmentVariableExplorer.Helpers
{
    public static class LocalizationOptions
    {
        public static readonly List<CultureInfo> SupportedCultures = new()
        {
            new CultureInfo("en-US"),
            new CultureInfo("fr-CA")
        };
    }
}
