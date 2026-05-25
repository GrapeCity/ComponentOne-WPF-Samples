using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using C1.Diagram.Parser;
using C1.WPF.Diagram;

namespace DiagramExplorer.Common
{
    /// <summary>
    /// Common stuff shared across samples.
    /// </summary>
    class Samples
    {
        static Stream? GetResourceStream(string name)
        {
            var asm = Assembly.GetExecutingAssembly();
            return asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.{name}");
        }

        public static void LoadDiagramFromResource(FlexDiagram diagram, string name)
        {
            var stream = GetResourceStream(name);

            var text = "";
            if (stream != null)
            {
                using (var sr = new StreamReader(stream))
                    text = sr.ReadToEnd();
            }

            
            diagram.LoadMermaidGraph(text);
        }

        public static Color Interpolate(Color clr1, Color clr2, double pos)
        {
            var a = clr1.A + (clr2.A - clr1.A) * pos;
            var r = clr1.R + (clr2.R - clr1.R) * pos;
            var g = clr1.G + (clr2.G - clr1.G) * pos;
            var b = clr1.B + (clr2.B - clr1.B) * pos;

            return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        }
    }
}
