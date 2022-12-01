using C1.WPF.Themes;

namespace ThemeExplorer.Helpers
{
    public enum C1AvailableThemes
    {
        Default,
        System,
        Material,
        MaterialDark
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
                default:
                    break;
            }
            return theme;
        }
    }

}
