using MudBlazor;

namespace EnvironmentVariableExplorer
{
    public class AppThemeProvider
    {
        private static MudTheme GetBaseTheme()
        {
            MudTheme theme = new MudTheme();

            theme.PaletteLight.Primary = "#0178D8";
            theme.PaletteLight.Secondary = "#232323";

            theme.PaletteDark.Primary = "#0178D8";
            theme.PaletteDark.Secondary = "#232323";

            return theme;
        }

        private static MudTheme GetLightTheme()
        {
            MudTheme theme = GetBaseTheme();

            theme.PaletteLight.Background = "#FAF9F6";
            theme.PaletteLight.DrawerBackground = theme.PaletteLight.Background;
            theme.PaletteLight.AppbarBackground = theme.PaletteLight.Primary;

            return theme;
        }

        private static MudTheme GetDarkTheme()
        {
            MudTheme theme = GetBaseTheme();

            theme.PaletteDark.AppbarBackground = theme.PaletteDark.DrawerBackground;

            return theme;
        }

        public static MudTheme GetTheme(bool isDarkMode)
        {
            return isDarkMode ? GetDarkTheme() : GetLightTheme();
        }
    }
}
