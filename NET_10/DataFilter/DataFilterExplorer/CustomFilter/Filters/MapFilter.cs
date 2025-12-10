using C1.DataCollection;
using C1.WPF.DataFilter;
using C1.WPF.Maps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DataFilterExplorer
{
    public class MapFilter : CustomFilter
    {
        private MapFilterPresenter _mapFilterPresenter;

        public MapFilter()
        {
            _mapFilterPresenter = new MapFilterPresenter();
            _mapFilterPresenter.SelectedChanged += MapFilterView_SelectedChanged;
            Control = _mapFilterPresenter;
        }

        private void MapFilterView_SelectedChanged(object sender, EventArgs e) => OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });

        public void SetStores(IEnumerable<Store> stores)
        {
            _mapFilterPresenter.SetStores(stores);
        }
        
        public override C1.WPF.DataFilter.Expression Expression 
        { 
            get
            {
                var stores = _mapFilterPresenter.GetSelectedStores();
                var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
                foreach (var store in stores)
                {
                    expr.Expressions.Add(new OperationExpression() { Value = store.ID, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
                }
                return expr;
            }
            set
            {
                var stores = GetStores(value).ToList();
                _mapFilterPresenter.SetSelectedStores(stores);
            }
        }

        private IEnumerable<Store> GetStores(C1.WPF.DataFilter.Expression expression)
        {
            if (expression is OperationExpression operation)
            {
                if (operation.PropertyName != PropertyName) yield break;

                var store = _mapFilterPresenter.Stores.FirstOrDefault(s => s.ID == (int)operation.Value);
                if (store != null)
                    yield return store;

            }
            else if (expression is CombinationExpression combination)
            {
                foreach (var e in combination.Expressions)
                {
                    foreach (var store in GetStores(e))
                    {
                        yield return store;
                    }
                }
            }
            yield break;
        }

        public override bool IsApplied => base.IsApplied && _mapFilterPresenter.GetSelectedStores().Any();
    }

    public class MapFilterPresenter : ItemsControl
    {
        private C1Maps _map;
        private VectorLayer _layer;
        private SolidColorBrush UnselectedBrush = new SolidColorBrush(Colors.LimeGreen);
        private SolidColorBrush SelectedBrush = new SolidColorBrush(Colors.Gold);
        public List<Store> Stores;
        public List<Store> SelectedStores;

        public MapFilterPresenter()
        {
            _map = new C1Maps
            {
                Background = new SolidColorBrush(Colors.LightGray),
                Height = 200,
                Width = 300,
                Zoom = 2,
                Center = new Point(-83, 39),
                ShowTools = false,
                Source = new VirtualEarthRoadSource(),
                MinZoom = 2,
                MaxZoom = 2
            };
            _map.MouseDoubleClick += _map_MouseDoubleClick;
            _layer = new VectorLayer { LabelVisibility = LabelVisibility.Visible };
            _map.Layers.Add(_layer);

            Items.Add(_map);
        }

        private void _map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public IEnumerable<Store> GetSelectedStores()
        {
            return Stores.Where(s => _layer.Children.Any(x => x.Fill == SelectedBrush && (string)x.Tag == s.City));
        }

        public event EventHandler SelectedChanged;

        public void SetStores(IEnumerable<Store> shops)
        {
            Stores = shops.ToList();
            foreach (var shop in shops)
            {
                var mark = new VectorPlacemark
                {
                    Tag = shop.City,
                    GeoPoint = new Point(shop.Location.X, shop.Location.Y),
                    Label = new TextBlock()
                    {
                        IsHitTestVisible = false,
                        Text = shop.City
                    },
                    LabelPosition = LabelPosition.Top,
                    Geometry = CreateBaloon(),
                    Fill = UnselectedBrush,
                    Opacity = 0.7
                };
                mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                _layer.Children.Add(mark);
            }
        }

        private void Mark_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pm = (VectorPlacemark)sender;
            if (pm == null)
            {
                return;
            }
            if (pm.Fill == UnselectedBrush)
            {
                pm.Fill = SelectedBrush;
            }
            else
            {
                pm.Fill = UnselectedBrush;
            }
            SelectedChanged?.Invoke(this, e);
        }

        private Geometry CreateBaloon()
        {
            PathGeometry pg = new PathGeometry
            {
                Transform = new TranslateTransform() { X = -10, Y = -24.14 }
            };
            PathFigure pf = new PathFigure() { StartPoint = new Point(10, 24.14), IsFilled = true, IsClosed = true };
            pf.Segments.Add(new ArcSegment() { SweepDirection = SweepDirection.Counterclockwise, Point = new Point(5, 19.14), RotationAngle = 45, Size = new Size(10, 10) });
            pf.Segments.Add(new ArcSegment() { SweepDirection = SweepDirection.Clockwise, Point = new Point(15, 19.14), RotationAngle = 270, Size = new Size(10, 10), IsLargeArc = true });
            pf.Segments.Add(new ArcSegment() { SweepDirection = SweepDirection.Counterclockwise, Point = new Point(10, 24.14), RotationAngle = 45, Size = new Size(10, 10) });
            pg.Figures.Add(pf);
            return pg;
        }

        internal void SetSelectedStores(List<Store> selectedStores)
        {
            foreach (var placemark in _layer.Children.OfType<VectorPlacemark>())
            {
                var isSelected = selectedStores?.FirstOrDefault(s => s.City == (string)placemark.Tag) != null;
                placemark.Fill = isSelected ? SelectedBrush : UnselectedBrush;
            }
        }
    }
}
