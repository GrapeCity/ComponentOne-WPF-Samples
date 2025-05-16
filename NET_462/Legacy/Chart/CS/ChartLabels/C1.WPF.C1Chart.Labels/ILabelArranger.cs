using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;

namespace C1.WPF.C1Chart.Labels
{
  /// <summary>
  /// Defines interface of label layout engine. 
  /// </summary>
  public interface ILabelArranger
  {
    /// <summary>
    /// Performs label layout.
    /// </summary>
    /// <param name="labels"></param>
    /// <param name="rectangles"></param>
    /// <param name="border"></param>
    void Arrange(IList<ILabel> labels, IList<Rect> rectangles, Rect border);
  }
}
