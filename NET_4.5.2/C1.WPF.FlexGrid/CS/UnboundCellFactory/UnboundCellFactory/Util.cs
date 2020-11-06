using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnboundCellFactory
{
    static class Util
    {
        // loads an image from an embedded resource
        public static BitmapImage LoadImage(string resourceName)
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = GetResourceStream(resourceName);
            img.EndInit();
            return img;
        }

        // gets a stream from an embedded resource
        public static Stream GetResourceStream(string name)
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (var rn in asm.GetManifestResourceNames())
            {
                if (rn.EndsWith(name, StringComparison.OrdinalIgnoreCase))
                {
                    return asm.GetManifestResourceStream(rn);
                }
            }
            throw new Exception("Resource not found: " + name);
        }

        // gets the first ancestor of an element that matches a given type
        public static object GetParentOfType(FrameworkElement e, Type type)
        {
            for (; e != null; e = VisualTreeHelper.GetParent(e) as FrameworkElement)
            {
                if (e.GetType().IsAssignableFrom(type))
                {
                    return e;
                }
            }
            return null;
        }
    }
}
