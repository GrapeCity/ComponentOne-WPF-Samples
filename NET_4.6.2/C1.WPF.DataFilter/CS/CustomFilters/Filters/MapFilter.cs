using C1.DataCollection;
using C1.DataFilter;
using C1.WPF.DataFilter;
using C1.WPF.Maps;
using CustomFilters.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomFilters
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

        protected override C1.DataFilter.Expression GetExpression()
        {
            var stores = _mapFilterPresenter.GetSelectedStores();
            var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
            foreach (var store in stores)
            {
                expr.Expressions.Add(new OperationExpression() { Value = store.ID, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
            }
            return expr;
        }

        protected override void SetExpression(C1.DataFilter.Expression expression)
        {
            
        }

        public override bool IsApplied => base.IsApplied && _mapFilterPresenter.GetSelectedStores().Count() > 0;
    }

    public class MapFilterPresenter: ItemsControl
    {
        private C1Maps _map;
        private C1VectorLayer _layer;
        private List<Store> _stores;

        public MapFilterPresenter()
        {
            _map = new C1Maps
            {
                Height = 200,
                Zoom = 2,
                Center = new Point(-83, 39),
                ShowTools = false,
                Source = new VirtualEarthRoadSource(),
                MinZoom = 2,
                MaxZoom = 2
            };
            _map.MouseDoubleClick += _map_MouseDoubleClick;
            _layer = new C1VectorLayer { LabelVisibility = LabelVisibility.Visible };
            _map.Layers.Add(_layer);

            Items.Add(_map);
        }

        private void _map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public IEnumerable<Store> GetSelectedStores()
        {
            return _stores.Where(s => _layer.Children.Any(x => ((SolidColorBrush)x.Fill).Color == new SolidColorBrush(Colors.Gold).Color && x.Tag == s.City));
        }

        public event EventHandler SelectedChanged;  

        public void SetStores(IEnumerable<Store> shops)
        {
            _stores = shops.ToList();
            foreach (var shop in shops)
            {
                var mark = new C1VectorPlacemark
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
                    Fill = new SolidColorBrush(Colors.LimeGreen), Opacity = 0.7
                };
                mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                _layer.Children.Add(mark);
            }
        }

        private void Mark_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pm = (C1VectorPlacemark)sender;
            if(pm == null)
            {
                return;
            }
            var limeGreen = new SolidColorBrush(Colors.LimeGreen);
            if (((SolidColorBrush)pm.Fill).Color == limeGreen.Color)
            {
                pm.Fill = new SolidColorBrush(Colors.Gold);
            }
            else
            {
                pm.Fill = limeGreen;
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
    }
}
