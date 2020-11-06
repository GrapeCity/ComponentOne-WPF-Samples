using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

using C1.WPF.C1Chart;

namespace C1.WPF.C1Chart.Labels
{
  /// <summary>
  /// Chart layer with labels.
  /// </summary>
  public class ChartLabels : Canvas, IChartLayer
  {
    #region fields

    ObservableCollection<ChartLabelBase> _children = new ObservableCollection<ChartLabelBase>();
    ObservableCollection<Rect> _occupiedRects = new ObservableCollection<Rect>();
    bool _loaded = false;
    ChartState _state = new ChartState();
    private int _updateCount = 0;
    ILabelArranger _arranger;
    C1Chart _chart;

    #endregion

    #region .ctor

    /// <summary>
    /// Creates an instance of ChartLabels class.
    /// </summary>
    public ChartLabels()
    {
      _children.CollectionChanged += new NotifyCollectionChangedEventHandler(_children_CollectionChanged);
      _occupiedRects.CollectionChanged += new NotifyCollectionChangedEventHandler(_occupiedRects_CollectionChanged);
      LayoutUpdated += new EventHandler(ChartLabels_LayoutUpdated);
      Loaded += new RoutedEventHandler(ChartLabels_Loaded);
    }

    #endregion

    #region Object model

    /// <summary>
    /// Gets the collection of chart labels.
    /// </summary>
    public new ObservableCollection<ChartLabelBase> Children
    {
      get { return _children; }
    }

    /// <summary>
    /// Gets or sets the label layout engine.
    /// </summary>
    public ILabelArranger LabelArranger
    {
      get { return _arranger; }
      set
      {
        if (value != _arranger)
        {
          if (_arranger is INotifyPropertyChanged)
            ((INotifyPropertyChanged)_arranger).PropertyChanged -= (ChartLabels_PropertyChanged);

          _arranger = value;

          if( _arranger is INotifyPropertyChanged)
            ((INotifyPropertyChanged)_arranger).PropertyChanged += (ChartLabels_PropertyChanged);
          Render();
        }
      }
    }

    /// <summary>
    /// Gets the collection of rectangles that represent occupied areas.
    /// </summary>
    public ObservableCollection<Rect> OccupiedRects
    {
      get { return _occupiedRects; }
    }

    void ChartLabels_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Render();
    }

    /// <summary>
    /// Starts batch updating.
    /// </summary>
    /// <remarks>
    /// Prevents updating of the control until the <see cref="EndUpdate"/> method is called.
    /// </remarks>
    public void BeginUpdate()
    {
      UpdateCount++;
    }

    /// <summary>
    /// Ends batch updating.
    /// </summary>
    /// <remarks>
    /// Updates the controls with changes made after the previous call of <see cref="BeginUpdate"/> method.
    /// </remarks>
    public void EndUpdate()
    {
      UpdateCount--;
    }

    #endregion

    #region Implementation

    C1Chart Chart
    {
      get{ return _chart;}
      set
      {
        if( value!=_chart)
          _chart = value;
      }
    }

    int UpdateCount
    {
      get { return _updateCount; }
      set
      {
        if (value <= 0)
        {
          _updateCount = 0;
          if(Chart!=null)
            Chart.LayoutUpdated += Chart_LayoutUpdated;
          Render();
        }
        else
          _updateCount = value;
      }
    }

    void Chart_LayoutUpdated(object sender, EventArgs e)
    {
      Chart.LayoutUpdated -= Chart_LayoutUpdated;
      Render();
    }

    void ChartLabels_Loaded(object sender, RoutedEventArgs e)
    {
      _loaded = true;
      Render();
    }

    void ChartLabels_LayoutUpdated(object sender, EventArgs e)
    {
      UpdateLabels();
    }

    void _children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      base.Children.Clear();

      if (_loaded)
        Render();
    }

    void _occupiedRects_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Render();
    }

    /// <summary>
    /// Arranges the labels.
    /// </summary>
    public void ForceArrange()
    {
      Render();
    }

    void UpdateLabels()
    {
      if (_loaded)
      {
        if (Chart != null)
        {
          var state = new ChartState(Chart);
          if (!state.Equals(_state))
          {
            Debug.WriteLine("ChartLabels_LayoutUpdates -> Render");
            _state = state;
            Render();
          }
        }
      }
    }

    void Render()
    {
      if (!_loaded || UpdateCount > 0)
        return;

      if (Chart != null)
      {
        var prect = Chart.View.PlotRect;
        var bnds = new Rect(0,0,prect.Width, prect.Height);
        bnds.Inflate(1, 1);

        var offset = Chart.TranslatePoint(new Point(), (UIElement)this.Parent);
        offset.X = prect.X - offset.X;
        offset.Y = prect.Y - offset.Y;

        foreach (var label in Children)
        {
          if (!base.Children.Contains(label))
            base.Children.Add(label);
          var line = label.ConnectingLine;
          if (!base.Children.Contains(line))
            base.Children.Add(line);


          if (!Chart.ChartType.ToString().StartsWith("Pie"))
          {
            label.UpdateAttachPoint(Chart, offset);

            if (bnds.Contains(label.AttachPoint))
              label.Visibility = Visibility.Visible;
            else
              label.Visibility = Visibility.Collapsed;
          }
          else
            label.UpdateAttachPoint(Chart, new Point());
        }

        if (LabelArranger != null)
          LabelArranger.Arrange(Children.ToArray(), OccupiedRects, new Rect(0, 0, prect.Width, prect.Height));

        foreach (var label in Children)
        {
          var line = label.ConnectingLine;
          var pt = label.AttachPoint;

          line.X1 = pt.X; line.Y1 = pt.Y;

          var pt1 = LabelHelper.GetConnectingPoint(label);

          if (!double.IsNaN(pt1.X) && !double.IsNaN(pt1.Y))
          {
            line.X2 = pt1.X; line.Y2 = pt1.Y;
          }
        }

        _state = new ChartState(Chart);
      }
    }

    #endregion

    #region IChartLayer
    
    C1Chart IChartLayer.Chart
    {
      get{ return Chart; }
      set{ Chart = value; }
    }

    #endregion
  }
}
