using System;
using System.Windows;

namespace C1.WPF.C1Chart.Labels
{
  /// <summary>
  /// Defines label interface.
  /// </summary>
  public interface ILabel
  {
    /// <summary>
    /// Gets the point the label attached to.
    /// </summary>
    Point Point { get; }

    /// <summary>
    /// Gets or sets the position(left-top corner) of label.
    /// </summary>
    Point Position { get; set; }

    /// <summary>
    /// Gets the size of label.
    /// </summary>
    Size Size { get; }

    /// <summary>
    /// Whether the label is visible.
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// Whether the label is movable.
    /// </summary>
    bool IsMovable { get; }

    /// <summary>
    /// Center of plot(e.g. pie center).
    /// </summary>
    Point PlotCenter { get; }
  }
}