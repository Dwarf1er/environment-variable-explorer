using System.Globalization;

namespace EnvironmentVariableExplorer.Models
{
    public class UserPreference
    {
        public string Id { get; set; }
        public bool IsDarkMode { get; set; }
        public string Language { get; set; }
    }
}
