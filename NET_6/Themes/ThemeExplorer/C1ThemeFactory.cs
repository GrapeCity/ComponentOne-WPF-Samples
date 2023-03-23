using C1.WPF.Themes;

namespace ThemeExplorer.Helpers
{
    public enum C1AvailableThemes
    {
        Default,
        System,
        Material,
        MaterialDark,
        MaterialOrange,
        MaterialOrangeDark,
        Custom
    }

    public static class C1ThemeFactory
    {

        public static C1Theme GetTheme(C1AvailableThemes pickedTheme)
        {
            C1Theme theme = null;
            switch (pickedTheme)
            {
                case C1AvailableThemes.Default:
                    theme = null;
                    break;
                case C1AvailableThemes.System:
                    theme = new C1ThemeSystem();
                    break;
                case C1AvailableThemes.Material:
                    theme = new C1ThemeMaterial();
                    break;
                case C1AvailableThemes.MaterialDark:
                    theme = new C1ThemeMaterialDark();
                    break;
                case C1AvailableThemes.Custom:
                    theme = new CustomTheme();
                    break;
                case C1AvailableThemes.MaterialOrange:
                    // create C1ThemeMaterial with custom overrides for theme colors
                    theme = new C1ThemeMaterial(typeof(C1ThemeFactory).Assembly, "ThemeExplorer.CustomTheme.MaterialOrange.xaml");
                    break;
                case C1AvailableThemes.MaterialOrangeDark:
                    // create C1ThemeMaterialDark with custom overrides for theme colors
                    theme = new C1ThemeMaterialDark(typeof(C1ThemeFactory).Assembly, "ThemeExplorer.CustomTheme.MaterialOrangeDark.xaml");
                    break;
                default:
                    break;
            }
            return theme;
        }
    }

}
