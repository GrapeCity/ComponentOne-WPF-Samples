using C1.WPF.Themes;

namespace ThemeExplorer.Helpers
{
    public enum C1AvailableThemes
    {
        Default,
        System,
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
                default:
                    break;
            }
            return theme;
        }
    }

}
