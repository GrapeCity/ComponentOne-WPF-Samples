using C1.WPF.Maps;
using System.IO;
using System.Collections.Generic;
namespace MapsSamples
{
    public delegate void ProcessShapeItem(C1VectorItemBase vector, C1ShapeAttributes attributes, Location location);

    public static class ShapeFileHelper
    {
        public static void LoadShape(this C1VectorLayer vl, Stream stream, Stream dbfStream, Location location, bool centerAndZoom,
      ProcessShapeItem processShape)
        {
            Dictionary<C1VectorItemBase, C1ShapeAttributes> vects = ShapeReader.Read(stream, dbfStream);

            vl.BeginUpdate();
            foreach (C1VectorItemBase vect in vects.Keys)
            {
                if (processShape != null)
                {
                    processShape(vect, vects[vect], location);
                }

                vl.Children.Add(vect);
            }
            vl.EndUpdate();
        }
    }
}
