using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;

namespace C1.WPF.C1Chart.Labels
{
  /// <summary>
  /// Represents chart state.
  /// </summary>
  class ChartState
  {
    List<double> _list = new List<double>();

    public ChartState()
    {
    }

    public ChartState(C1Chart chart)
    {
      PlotRect = chart.View.PlotRect;

      foreach (var ax in chart.View.Axes)
      {
        _list.Add(ax.Min);
        _list.Add(ax.Max);
        _list.Add(ax.ActualMin);
        _list.Add(ax.ActualMax);
        _list.Add(ax.Scale);
        _list.Add(ax.Value);
      }
    }

    public Rect PlotRect;

    public override bool Equals(object obj)
    {
      var state = (ChartState)obj;
      if (PlotRect != state.PlotRect)
        return false;

      int cnt = _list.Count;
      if(cnt!=state._list.Count)
        return false;

      for (int i = 0; i < cnt; i++)
      {
        if (double.IsNaN(_list[i]) && double.IsNaN(state._list[i]))
          continue;
        
        if (_list[i] != state._list[i])
          return false;
      }

      return true;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
