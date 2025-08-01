using EnvironmentVariableExplorer.Services;
using EnvironmentVariableExplorer.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace EnvironmentVariableExplorer.Components.Layout
{
    public partial class MainLayout : LocalizedComponentBase<MainLayout>
    {
        [Inject] private UserPreferenceService UserPreferenceService { get; set; }

        public required MudThemeProvider _mudThemeProvider;
        private MudTheme _mudTheme;
        private bool _isDarkMode;
        private string _language;

        private async Task ThemeToggle()
        {
            _isDarkMode = !_isDarkMode;
            await UserPreferenceService.StoreUserPreferenceAsync(_isDarkMode, _language);
            _mudTheme = AppThemeProvider.GetTheme(_isDarkMode);
            StateHasChanged();
        }

        private async Task LanguageToggle()
        {
            _language = (_language == "FR") ? "EN" : "FR";
            LanguageService.SetLanguage(_language);
            await UserPreferenceService.StoreUserPreferenceAsync(_isDarkMode, _language);
            StateHasChanged();
        }

        private string GetThemeIcon()
        {
            return _isDarkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _isDarkMode = await UserPreferenceService.GetUserDarkModePreferenceAsync();
            _language = await UserPreferenceService.GetUserLanguagePreferenceAsync();

            if (_language == null)
            {
                _language = SystemUtils.GetSystemLanguage();
            }

            LanguageService.SetLanguage(_language);
            _mudTheme = AppThemeProvider.GetTheme(_isDarkMode);

            await _mudThemeProvider.WatchSystemDarkModeAsync(OnSystemPreferenceChanged);

            StateHasChanged();
        }

        private Task OnSystemPreferenceChanged(bool newValue)
        {
            _isDarkMode = newValue;
            _mudTheme = AppThemeProvider.GetTheme(_isDarkMode);
            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}