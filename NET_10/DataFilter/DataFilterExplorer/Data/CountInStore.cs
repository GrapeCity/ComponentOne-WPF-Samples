using System.Windows.Media;

namespace DataFilterExplorer
{
    public class CountInStore
    {
        public Car Car { get; set; }
        public int Count { get; set; }
        public Store Store { get; set; }
        public Color Color { get; set; }

        public SolidColorBrush BrushColor => new(Color);
    }
}
