using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using C1.WPF;
using C1.WPF.Extended;
using System.Reflection;
using C1.WPF.Extended.PropertyGrid;
using System.Windows.Input;

namespace ControlExplorer
{
    public partial class SamplePage : Page
    {
        public SamplePage()
        {
            InitializeComponent();
        }

        public void OnNavigatedTo(string fragment)
        {
            fragment = "/" + Uri.UnescapeDataString(fragment);
            var args = fragment.Split('/');
            FeatureDescription feature = null;
            if (args.Length > 1)
            {
                var control = MainViewModel.Instance.Controls.FirstOrDefault(c => c.Name.Equals(args[1]));
                if (control != null)
                    feature = control.GetAllFeatures().FirstOrDefault(f => f.Link.OriginalString == fragment);
            }
            else
            {
                feature = MainViewModel.Instance.Features.FirstOrDefault(f => f.Link.OriginalString == fragment);
            }
            if (feature.SubFeatures.Count != 0)
            {
                FeatureDescription toSelect = feature.SubFeatures[0];
                foreach(var f in feature.SubFeatures)
                {
                    if ( f.IsNew)
                    {
                        toSelect = f;
                    }
                }
                feature.IsExpanded = true;
                feature = toSelect;
            }
            if (feature != null)
            {
                try
                {
                    Title = feature.Control.Name + " - " + feature.Name;
                    var assembly = Assembly.LoadFrom(feature.AssemblyName);
                    feature.Sample = assembly.CreateInstance(feature.FullDemoControlTypeName) as FrameworkElement;
                    properties.AvailableEditors.Insert(0, new TimeSpanEditor());
                    properties.AvailableEditors.Insert(0, new NumericEditor());
                    properties.AvailableEditors.Insert(0, new ReadOnlyImageSourceEditor());
                    properties.AvailableEditors.Insert(0, new CornerRadiusEditor());
                    properties.PropertyAttributes.Clear();
                    LoadCollection<PropertyAttribute>(properties.PropertyAttributes, feature.Properties);
                    DataContext = feature;
                    if (feature.Sample == null)
                    {
                        code.ContainsSample = false;
                    }
                    else
                    {
                        code.ContainsSample = true;
                        code.Source = feature.Source;
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.UpdateLayout();
                            var mask = properties.SelectedObject as BasicControls.DemoMaskedTextBox;
                            if (mask != null)
                            {
                                foreach (PropertyBox box in properties.PropertyBoxes)
                                {
                                    if (box.CurrentEditor != null && box.CurrentEditor is StringEditor && box.PropertyAttribute.DisplayName == "Text")
                                    {
                                        TextBox txt = box.CurrentEditor as TextBox;
                                        txt.PreviewTextInput += txt_PreviewTextInput;
                                        txt.TextChanged += txt_TextChanged;
                                        break;
                                    }
                                }
                            }
                            var item = FindItem(tree, feature);

                            if (item != null)
                            {
                                item.IsSelected = true;
                                IList<DependencyObject> list = new List<DependencyObject>();
                                VTreeHelper.GetChildrenOfType(tree, typeof(ScrollViewer), ref list);
                                if (list.Count > 0)
                                {
                                    ScrollViewer scv = list[0] as ScrollViewer;
                                    Point point = item.TransformToVisual(scv).Transform(new Point(scv.HorizontalOffset, scv.VerticalOffset));
                                    Rect rect = new Rect(point, new Point(point.X + item.ActualWidth, point.Y + item.ActualHeight));
                                    if (scv.VerticalOffset > rect.Top)
                                    {
                                        scv.ScrollToVerticalOffset(rect.Top);
                                    }
                                    else if (scv.VerticalOffset < rect.Bottom - scv.ViewportHeight)
                                    {
                                        scv.ScrollToVerticalOffset(rect.Bottom - scv.ViewportHeight);
                                    }
                                }
                            }
                        }), System.Windows.Threading.DispatcherPriority.Loaded);
                }
                catch (Exception exc)
                {
                    if (exc.InnerException != null &&
                        exc.InnerException.Message != null &&
                        exc.InnerException.Message.Contains(ControlExplorer.Properties.Resources.SamplePage_ExceptionText))
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show(ControlExplorer.Properties.Resources.SamplePage_MsgBoxText);
                        }), null);
                    }
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                }
            }
        }


        protected void OnNavigatedFrom(NavigationEventArgs e)
        {
            FeatureDescription feature = DataContext as FeatureDescription;
            if (feature != null)
            {
                IDisposable disposable = feature.Sample as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private C1TreeViewItem FindItem(C1HierarchicalPresenter tree, FeatureDescription feature)
        {
            var treeViewItem = tree.ItemContainerGenerator.ContainerFromItem(feature) as C1TreeViewItem;
            if (treeViewItem != null)
            {
                return treeViewItem;
            }
            else
            {
                foreach (var item in tree.Items)
                {
                    var tvi = tree.ItemContainerGenerator.ContainerFromItem(item) as C1TreeViewItem;
                    if (tvi != null && tvi.HasItems)
                    {
                        treeViewItem = FindItem(tvi, feature);
                        if (treeViewItem != null)
                            return treeViewItem;
                    }
                }
            }
            return null;
        }

        private void LoadCollection<T>(Collection<T> list, IEnumerable data)
        {
            if (data != null)
                foreach (var item in data)
                {
                    list.Add((T)item);
                }
        }

        private void tree_ItemPrepared(object sender, C1.WPF.ItemPreparedEventArgs e)
        {
            (e.Element as C1TreeViewItem).SetBinding(C1TreeViewItem.IsExpandedProperty, new Binding("IsExpanded") { Source = e.Item, Mode = BindingMode.TwoWay });
        }

        private void tree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                var feature = (e.AddedItems[0] as C1TreeViewItem).DataContext as FeatureDescription;
                try
                {
                    if (feature.SubFeatures.Count() == 0)
                    {
                        NavigationService.Navigate(feature.Link);
                    }
                    else
                    {
                        feature.IsExpanded = !feature.IsExpanded;
                    }
                }
                catch { }
            }
        }

        private void properties_AddingPropertyBox(object sender, ChangingPropertyBoxEventArgs e)
        {
            e.PropertyBox.Style = Resources["PropertiesPropertyBox"] as Style;
        }

        char[] maskElements = new char[] { '0', '9', '#', 'L', '?', '&', 'C', 'A', 'a', '.', ',', ':', '/', '$', '<', '>', '|', '\\' };
        int previewCaret;
        bool updateInTextChanged;
        void txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            int inputLength = e.Text.Length;
            TextBox tb = sender as TextBox;
            string txt = tb.Text;
            string mask = (properties.PropertyBoxes[3].CurrentEditor as TextBox).Text;
            if (txt.Length < mask.Length)
            {
                updateInTextChanged = true;
                previewCaret = inputLength;
                return;
            }
            int caret = tb.CaretIndex;
            this.previewCaret = CreateCurrentCaret(txt, mask, tb.CaretIndex, (properties.SelectedObject as BasicControls.DemoMaskedTextBox).PromptChar) + inputLength;
        }
        void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            string txt = tb.Text;
            string mask = (properties.PropertyBoxes[3].CurrentEditor as TextBox).Text;
            if (updateInTextChanged)
            {
                tb.CaretIndex = CreateCurrentCaret(txt, mask, 0, (properties.SelectedObject as BasicControls.DemoMaskedTextBox).PromptChar)+previewCaret;
                this.previewCaret = -1;
                updateInTextChanged = false;
            }
            else if (this.previewCaret != -1)
            {
                tb.CaretIndex = this.previewCaret;
                this.previewCaret = -1;
            }
        }
      
        int CreateCurrentCaret(string txt,string mask, int currentCaret, char promptChar)
        {
            int count = 0;
            for (int i = currentCaret; i < txt.Length; i++)
            {
                if (txt[i] == promptChar)
                    return currentCaret + count;
                else if (i>=mask.Length)
                    return currentCaret + count;
                else if (txt[i] == mask[i])
                {
                    count++;
                }
                else
                    return currentCaret + count;
            }
            return currentCaret + count;
        }

        private void C1TabItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete) return;

            var tab = sender as C1TabItem;
            if (tab == null) return;

            var fd = tab.DataContext as FeatureDescription;
            if (fd == null) return;

            if (fd.Sample.GetType() == typeof(FlexChartShowcase.ShowcaseControl))
            {
                ((FlexChartShowcase.ShowcaseControl)fd.Sample).HandleKeyDown(e);
            }
        }
    }
}
