using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace C1ExpressionEditorSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Deactivated += OnDeactivated;
        }

        private void OnDeactivated(object sender, EventArgs e)
        {
            foreach(Popup popup in GetChildren<Popup>(MainWindow))
            {
                if (popup.IsOpen)
                    popup.IsOpen = false;
            }
        }

        private IEnumerable<T> GetChildren<T>(FrameworkElement elem)
    where T : FrameworkElement
        {
            return GetChildren(elem).Where(e => e is T).Select(e => e as T);
        }

        private IEnumerable<DependencyObject> GetChildren(FrameworkElement elem, bool includeSelf = true, bool recursive = true)
        {
            if (elem == null) yield break;
            if (includeSelf) yield return elem;

            FrameworkElement current = elem;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(elem); i++)
            {
                var child = VisualTreeHelper.GetChild(elem, i);
                yield return child;
                if (recursive && child is FrameworkElement)
                {
                    foreach (var item in GetChildren(child as FrameworkElement, false, true))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
