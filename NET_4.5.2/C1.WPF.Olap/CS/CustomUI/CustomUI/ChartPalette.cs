using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomUI
{
    public static class ChartPalette
    {
        public static C1.WPF.C1Chart.ColorGeneration GetPalette(String palette)
        {
            if (palette.Equals("Apex"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Apex;
            }
            else if (palette.Equals("Aspect"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Aspect;
            }
            else if (palette.Equals("Civic"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Civic;
            }
            else if (palette.Equals("Concourse"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Concourse;
            }
            else if (palette.Equals("Equity"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Equity;
            }
            else if (palette.Equals("Flow"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Flow;
            }
            else if (palette.Equals("Foundry"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Foundry;
            }
            else if (palette.Equals("GrayScale"))
            {
                return C1.WPF.C1Chart.ColorGeneration.GrayScale;
            }
            else if (palette.Equals("Median"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Median;
            }
            else if (palette.Equals("Metro"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Metro;
            }
            else if (palette.Equals("Module"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Module;
            }
            else if (palette.Equals("Office"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Office;
            }
            else if (palette.Equals("Opulent"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Opulent;
            }
            else if (palette.Equals("Oriel"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Oriel;
            }
            else if (palette.Equals("Origin"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Origin;
            }
            else if (palette.Equals("Paper"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Paper;
            }
            else if (palette.Equals("Solstice"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Solstice;
            }
            else if (palette.Equals("Standard"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Standard;
            }
            else if (palette.Equals("Technic"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Technic;
            }
            else if (palette.Equals("Trek"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Trek;
            }
            else if (palette.Equals("Urban"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Urban;
            }
            else if (palette.Equals("Verve"))
            {
                return C1.WPF.C1Chart.ColorGeneration.Verve;
            }
            else
            {
                return C1.WPF.C1Chart.ColorGeneration.Default;
            }
        }

    }

}
