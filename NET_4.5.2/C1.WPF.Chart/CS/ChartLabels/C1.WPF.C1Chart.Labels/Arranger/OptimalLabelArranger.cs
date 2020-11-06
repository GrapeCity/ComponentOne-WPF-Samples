using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
  /// <summary>
  /// Engine for label layout.
  /// </summary>
  public class OptimalLabelArranger : BaseLabelArranger, INotifyPropertyChanged
  {
    #region fields
    
    // weight of point conflicts
    const int wPointConflict = 8;
    
    // weight of rectangle conflicts
    const int wRectConflict = 2;

    // weight of bounds conflicts
    const int wBorderConflict = 2;

    // weight of fixed labels conflicts
    const int wFixedConflict = 8;

    // maximum number of iterations
    const int maxIterations = 150;

    Random rnd = new Random();

    IList<int> _angles;

    double _maxOffset;

    List<ILabel> _hiddenLabels = new List<ILabel>();

    bool _hideOutside = false;
    
    #endregion

    #region .ctor
    
    /// <summary>
    /// Creates an instance of OptimalLabelArranger.
    /// </summary>
    public OptimalLabelArranger()
    {
      MaxOffset = 40;
    }
    
    #endregion

    #region object model

    /// <summary>
    /// Gets or sets the collection of allowable angles when positioning labels.
    /// </summary>
    public IList<int> Angles 
    {
      get { return _angles;}
      set
      {
        if (value != _angles)
        {
          _angles = value;
          OnPropertyChanged("Angles");
        }
      }
    }

    /// <summary>
    /// Gets or sets the maximum label offset from the corresponding point.
    /// </summary>
    public double MaxOffset 
    { 
      get { return _maxOffset;}
      set
      {
        if (value != _maxOffset)
        {
          _maxOffset = value;
          OnPropertyChanged("MaxOffset");
        }
      }
    }

    /// <summary>
    /// If true the arranger hides labels when they are outside border.
    /// </summary>
    public bool HideLabelsOutsideBorder
    {
      get { return _hideOutside; }
      set
      {
        if (value != _hideOutside)
        {
          _hideOutside = value;
          OnPropertyChanged("HideLabelsOutsideBorder");
        }
      }
    }

    #endregion

    #region ILabelArranger
    
    /// <summary>
    /// Performs layout of labels.
    /// </summary>
    /// <param name="labels"></param>
    /// <param name="rectangles"></param>
    /// <param name="border"></param>
    public override void Arrange(IList<ILabel> labels, IList<Rect> rectangles, Rect border)
    {
      if (labels == null)
        throw new ArgumentNullException();

      if (_hiddenLabels.Count > 0)
      {
        foreach (var lbl in _hiddenLabels)
          lbl.IsVisible = true;

        _hiddenLabels.Clear();
      }

      var current_state = CreateState(labels);
      LabelState[] best_state = current_state;
      int best_energy = int.MaxValue;

      int cnt = labels.Count;

      int iteration = 0;
      for (; iteration < maxIterations; iteration++)
      {
        for (int i = 0; i < cnt; i++)
        {
          if (iteration == 0)
          {
            //current_state[i].Energy = 0;
            //Place(current_state[i]);
          }
          else if (current_state[i].Energy > 0)
          {
            current_state[i].Energy = 0;
            Place(current_state[i]);
          }
        }
        
        var energy = GetEnergy(current_state, border);

        if( energy < best_energy)
        {
          best_energy = energy;
          best_state = current_state;
        }

        if (best_energy == 0)
          break;

        current_state = CreateState(current_state);
      }

      Debug.WriteLine("Energy = " + best_energy + ", Iteration = " + iteration);

      for (int i = 0; i < cnt; i++)
      {
        best_state[i].CopyState(labels[i]);

        if (HideLabelsOutsideBorder && best_state[i].IsOutsideBorder)
        {
          if (labels[i].IsVisible)
          {
            labels[i].IsVisible = false;
            _hiddenLabels.Add(labels[i]);
          }
        }
      }
    }
    #endregion

    #region implementation
   
    void Place(LabelState label)
    {
      if( label.IsVisible && label.IsMovable)
      {
        var sz = label.Size;
        var min_offset = Math.Sqrt(sz.Width * sz.Width + sz.Height * sz.Height);

        var range = MaxOffset - min_offset;
        if (range < 0)
          range = 0;
        var offset = min_offset + rnd.NextDouble() * range;
        
        double angle = 0;

        if (Angles != null && Angles.Count > 0)
          angle = Angles[rnd.Next(Angles.Count)] * Math.PI / 180;
        else
          angle = rnd.NextDouble() * Math.PI * 2;

        var center = new Point(label.Point.X + offset * Math.Cos(angle), label.Point.Y + offset * Math.Sin(angle));

        label.Position = new Point(center.X - 0.5 * sz.Width, center.Y - 0.5 * sz.Height);
      }
    }

    int GetEnergy(IList<LabelState> lbls, Rect border)
    {
      return GetPointConflicts(lbls) + GetRectConflicts(lbls) + GetBorderConflicts(lbls, border)
        // + GetConnectionConflicts(lbls)
        ; 
    }

    int GetRectConflicts(IList<LabelState> lbls)
    {
      int ncon = 0;

      int len = lbls.Count;
      for (int i = 0; i < len; i++)
      {
        if (lbls[i].IsVisible)
        {
          for (int j = 0; j < len; j++)
          {
            if (i != j && lbls[j].IsVisible)
            {
              if ( Intersect( lbls[j], lbls[i]))
              {
                lbls[i].Energy += wRectConflict;
                lbls[j].Energy += wRectConflict;
                ncon++;
              }
            }
          }
        }
      }

      return ncon * wRectConflict;
    }

    int GetPointConflicts(IList<LabelState> lbls)
    {
      int ncon = 0;

      int len = lbls.Count;
      for (int i = 0; i < len; i++)
      {
        if (lbls[i].IsVisible)
        {
          for (int j = 0; j < len; j++)
          {
            if (i != j)
            {
              if (IntersectPoint(lbls[j], lbls[i].Point))
              {
                lbls[j].Energy += wPointConflict;
                ncon++;
              }
            }
          }
        }
      }

      return wPointConflict * ncon;
    }

    int GetConnectionConflicts(IList<LabelState> lbls)
    {
      int ncon = 0;

      int len = lbls.Count;
      for (int i = 0; i < len; i++)
      {
        if (lbls[i].IsVisible)
        {
          for (int j = 0; j < len; j++)
          {
            if (i != j && lbls[j].IsVisible)
            {
              if (!LabelHelper.Intersect(lbls[i].Point, lbls[i].GetConnectingPoint(), lbls[j].Point, lbls[j].GetConnectingPoint()).IsEmpty())
              {
                lbls[i].Energy += wRectConflict;
                lbls[j].Energy += wRectConflict;
                ncon++;
              }
            }
          }
        }
      }

      return ncon * wRectConflict;
    }


    int GetBorderConflicts(IList<LabelState> lbls, Rect bounds)
    {
      int ncon = 0;

      if (!bounds.IsEmpty)
      {
        int len = lbls.Count;
        for (int i = 0; i < len; i++)
        {
          if (lbls[i].IsVisible)
          {
            if (!Contains(lbls[i], bounds))
            {
              lbls[i].Energy += wBorderConflict;
              lbls[i].IsOutsideBorder = true;
              ncon++;
            }
            else
              lbls[i].IsOutsideBorder = false;
          }
        }
      }

      return ncon * wBorderConflict;
    }
    
    #endregion

    #region INotifyPropertyChanged

    /// <summary>
    /// Fires when property was changed.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
