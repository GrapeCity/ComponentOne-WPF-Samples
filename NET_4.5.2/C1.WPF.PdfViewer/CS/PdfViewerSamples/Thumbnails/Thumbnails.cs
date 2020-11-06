using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using C1.WPF.PdfViewer;
using System.Windows.Media;
using System.Windows.Data;
using System.Threading;
using System.Windows.Threading;

namespace PdfViewerSamples
{
    [TemplatePart(Name = "ScrollViewer", Type = typeof(ScrollViewer))]
    public class Thumbnails
        : ItemsControl
    {
        ScrollViewer _scrollViewer;
        ThumbnailItem _lastSelectedChild = null;

        public Thumbnails()
        {
            this.DefaultStyleKey = typeof(Thumbnails);
        }


        #region ** overrides

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ThumbnailItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ThumbnailItem);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var container = (ThumbnailItem)element;
            if (element != item)
            {
                container.SetBinding(ThumbnailItem.ScaleProperty, new System.Windows.Data.Binding("ItemScale") { Source = this });
                container.Content = item;

                if (_lastSelectedChild == null)
                    this.SelectItem(container);

                container.MouseLeftButtonDown -= new MouseButtonEventHandler(container_MouseLeftButtonDown);
                container.MouseLeftButtonDown += new MouseButtonEventHandler(container_MouseLeftButtonDown);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _scrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");
        }


        #endregion

        #region ItemScale

        public double ItemScale
        {
            get { return (double)GetValue(ItemScaleProperty); }
            set { SetValue(ItemScaleProperty, value); }
        }
        public static readonly DependencyProperty ItemScaleProperty =
            DependencyProperty.Register("ItemScale", typeof(double), typeof(Thumbnails), new PropertyMetadata(1.0));

        #endregion

        #region PdfViewer

        public C1PdfViewer PdfViewer
        {
            get { return (C1PdfViewer)GetValue(PdfViewerProperty); }
            set { SetValue(PdfViewerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PdfViewer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PdfViewerProperty =
            DependencyProperty.Register("PdfViewer", typeof(C1PdfViewer), typeof(Thumbnails), new PropertyMetadata(null, OnPdfViewerChanged));

        static void OnPdfViewerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((Thumbnails)sender).OnPdfViewerChanged(args.OldValue as C1PdfViewer, args.NewValue as C1PdfViewer);
        }

        #endregion

        #region SelectedBrush

        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(Thumbnails), new PropertyMetadata(null));

        #endregion

        private void OnPdfViewerChanged(C1PdfViewer oldPdf, C1PdfViewer newPdf)
        {
            if (newPdf != null)
            {
                ItemsSource = newPdf.GetPages();
            }
            else
            {
                ItemsSource = null;
            }
        }

        void container_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // handle selected page
            SelectItem((ThumbnailItem)sender);

            // scroll entirely into view
            var childIndex = ItemContainerGenerator.IndexFromContainer((ThumbnailItem)sender);
            var pageSize = _scrollViewer.ExtentHeight / PdfViewer.PageCount;
            var verticalChildStart = pageSize * childIndex;
            var verticalChildEnd = pageSize * (childIndex + 1);
            if (_scrollViewer.VerticalOffset > verticalChildStart)
            {
                _scrollViewer.ScrollToVerticalOffset(verticalChildStart);
            }
            else if (_scrollViewer.VerticalOffset + _scrollViewer.ViewportHeight < verticalChildEnd)
            {

                _scrollViewer.ScrollToVerticalOffset(verticalChildStart - _scrollViewer.ViewportHeight + pageSize);
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                PdfViewer.GoToPage(1 + childIndex);
            }));
        }


        private void SelectItem(ThumbnailItem newChild)
        {
            if (_lastSelectedChild != null)
            {
                _lastSelectedChild.ClearValue(BackgroundProperty);
            }

            if (newChild != null)
            {
                newChild.SetBinding(BackgroundProperty, new Binding("SelectedBrush") { Source = this });
            }
            _lastSelectedChild = newChild;
        }
    }
}
