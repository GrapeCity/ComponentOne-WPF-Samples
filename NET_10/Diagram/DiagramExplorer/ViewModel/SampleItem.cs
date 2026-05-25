using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DiagramExplorer.Resources;

using C1.WPF.Diagram;

# pragma warning disable 1591
namespace DiagramExplorer.ViewModel
{
    interface IDiagramHolder
    {
        FlexDiagram Diagram { get; }
    }

    public interface ISampleItem
    {
        string? Name { get; }
        string? Title { get; }
        string? Description { get; }
        UserControl? Sample { get; }

        FlexDiagram? Diagram { get; }

        public string Controls { get; }
    }

    public class SampleGroup : ISampleItem
    {
        public string? Name { get; set; }
        public string? Title => Children?[0].Title;
        public string? Description => Children?[0].Description;
        public UserControl? Sample => Children?[0].Sample;

        public List<ISampleItem>? Children { get; set; }

        public FlexDiagram? Diagram => null;

        public string Controls { get; set; } = "";
    }

    public class SampleItem<T> : ISampleItem where T : UserControl, new()
    {
        private UserControl? sample;

        string TypeName => typeof(T).Name;

        public string Name
        {
            get
            {
                var name = AppResources.ResourceManager.GetString(TypeName + "Title");
                return string.IsNullOrEmpty(name) ? TypeName : name;
            }
        }
        public string Title
        {
            get
            {
                var name = AppResources.ResourceManager.GetString(TypeName + "Title");
                return string.IsNullOrEmpty(name) ? TypeName : name;
            }
        }
        public string Header
        {
            get
            {
                var header = AppResources.ResourceManager.GetString(TypeName + "Header");
                return string.IsNullOrEmpty(header) ? Title : header;
            }
        }

        public string? Description => AppResources.ResourceManager.GetString(typeof(T).Name + "Description");
        public UserControl Sample
        {
            get
            {
                if (sample == null)
                    sample = new T();
                return sample;
            }
        }

        public FlexDiagram? Diagram
        {
            get => (Sample as IDiagramHolder)?.Diagram;
        }

        public List<ISampleItem>? Children => null;

        public string Controls { get; set; } = "Direction,EdgeRouting";
    }
}
