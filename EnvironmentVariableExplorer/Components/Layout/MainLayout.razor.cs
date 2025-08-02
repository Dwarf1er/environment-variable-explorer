using EnvironmentVariableExplorer.Helpers;
using EnvironmentVariableExplorer.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;
using System.Threading.Tasks;

namespace EnvironmentVariableExplorer.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] private UserPreferenceService UserPreferenceService { get; set; }
        [Inject] private LanguageService LanguageService {  get; set; }

        public required MudThemeProvider _mudThemeProvider;
        private MudTheme _mudTheme;
        private bool _isDarkMode;
        private CultureInfo _culture = CultureInfo.CurrentCulture;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            LanguageService.OnLanguageChanged += LanguageChanged;

            _isDarkMode = await UserPreferenceService.GetUserDarkModePreferenceAsync();
            _culture = await UserPreferenceService.GetUserLanguagePreferenceAsync();

            LanguageService.SetLanguage(_culture);
            _mudTheme = AppThemeProvider.GetTheme(_isDarkMode);

            await _mudThemeProvider.WatchSystemDarkModeAsync(OnSystemPreferenceChanged);

            StateHasChanged();
        }

        private async Task ThemeToggle()
        {
            _isDarkMode = !_isDarkMode;
            await UserPreferenceService.StoreUserPreferenceAsync(_isDarkMode, _culture);
            _mudTheme = AppThemeProvider.GetTheme(_isDarkMode);
            StateHasChanged();
        }

        private async Task OnCultureChanged(CultureInfo newCulture)
        {
            _culture = newCulture;
            LanguageService.SetLanguage(newCulture);
            await UserPreferenceService.StoreUserPreferenceAsync(_isDarkMode, newCulture);
        }

        private void LanguageChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        private string GetThemeIcon()
        {
            return _isDarkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;
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