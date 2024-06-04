using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace C1.WPF.C1Chart.Labels
{
  class LabelState : ILabel
  {
    public LabelState(ILabel label)
    {
      Point = label.Point; Position = label.Position; Size = label.Size; 
      IsVisible = label.IsVisible; IsMovable = label.IsMovable; PlotCenter = label.PlotCenter;

      if (label is LabelState)
        Energy = ((LabelState)label).Energy;
    }

    public Point Point
    {
      get; set;
    }

    public Point Position
    {
      get; set;
    }

    public Size Size
    {
      get; set;
    }

    public bool IsVisible
    {
      get; set;
    }

    public bool IsMovable
    {
      get;
      set;
    }

    public void CopyState(ILabel label)
    {
      label.Position = Position;
    }

    public Point PlotCenter
    {
      get;
      private set;
    }

    internal int Energy { get; set; }

    internal bool IsOutsideBorder { get; set; }

    internal double DistanceFromBorder { get; set; }
  }
}
