using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using FlexViewerExplorer.Resources;
using C1.WPF.Viewer;
using System.IO;
using System.Reflection;
using C1.WPF.Document;
using Microsoft.Win32;
using System.Windows.Input;

namespace FlexViewerExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomAppearance : UserControl
    {
        C1PdfDocumentSource pdfDocumentSource = new C1PdfDocumentSource();
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
            Title = "Select the PDF file",
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
        };

        public CustomAppearance()
        {
            this.InitializeComponent();

            Tag = AppResources.CustomAppearanceDesc;

            viewer.DocumentSource = pdfDocumentSource;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FlexViewerExplorer.Resources.Simple List.pdf");
            pdfDocumentSource.LoadFromStream(stream);

            InitUI();
            Loaded += CustomAppearance_Loaded;
        }

        private void CustomAppearance_Loaded(object sender, RoutedEventArgs e)
        {
            PageNumberTextBox.SetBinding(TextBox.TextProperty,
                new Binding() { Source = viewer, Path = new PropertyPath(nameof(FlexViewerPane.PageNumber)), Mode = BindingMode.OneWay });
            PageCountText.SetBinding(TextBlock.TextProperty,
                new Binding() { Source = viewer, Path = new PropertyPath(nameof(FlexViewerPane.PageCount)) });
        }

        void InitUI()
        {
            OpenButton.Click += OpenButton_Click;
            CloseButton.Click += CloseButton_Click;
            ExportButton.Click += ExportButton_Click;
            PrintButton.Click += PrintButton_Click;
            RefreshButton.Click += RefreshButton_Click;
            PageNumberTextBox.KeyDown += PageNumberTextBox_KeyDown;
            FirstPageButton.Click += FirstPageButton_Click;
            PreviousPageButton.Click += PreviousPageButton_Click;
            NextPageButton.Click += NextPageButton_Click;
            LastPageButton.Click += LastPageButton_Click;

            ZoomCombo.ItemConverter = new FlexViewerZoomTypeConverter();
            var list = new List<object>();
            list.Add(FlexViewerZoomMode.ActualSize);
            list.Add(FlexViewerZoomMode.PageWidth);
            list.Add(FlexViewerZoomMode.WholePage);
            //list.AddRange(FixedZooms.Cast<object>());
            ZoomCombo.ItemsSource = list;
            //switch (_fv.Pane.ZoomMode)
            switch (viewer.ZoomMode)
            {
                case FlexViewerZoomMode.ActualSize:
                    ZoomCombo.SelectedIndex = 0;
                    break;
                case FlexViewerZoomMode.PageWidth:
                    ZoomCombo.SelectedIndex = 1;
                    break;
                case FlexViewerZoomMode.WholePage:
                    ZoomCombo.SelectedIndex = 2;
                    break;
            }
            ZoomCombo.SelectedItemChanged += ZoomCombo_SelectedItemChanged;
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.PageNumber = viewer.DocumentSource.PageCount;
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.PageNumber = viewer.PageNumber + 1;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.PageNumber = viewer.PageNumber - 1;
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.PageNumber = 1;
        }

        private void PageNumberTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var fvp = viewer;
            if (e.Key == Key.Escape)
            {
                PageNumberTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                int i;
                if (int.TryParse(PageNumberTextBox.Text, out i))
                {
                    fvp.PageNumber = i;
                }
                e.Handled = true;
            }
        }

        private void ZoomCombo_SelectedItemChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
                viewer.ZoomMode = (FlexViewerZoomMode)e.NewValue;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.Reflow();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.Print();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.Export();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            pdfDocumentSource.ClearContent();
            viewer.Focus();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.Focus();

            if (!openFileDialog.ShowDialog().Value)
                return;

            // load document
            while (true)
            {
                try
                {
                    pdfDocumentSource.LoadFromFile(openFileDialog.FileName);
                    break;
                }
                catch (PdfPasswordException)
                {
                    var password = PasswordWindow.DoEnterPassword(openFileDialog.FileName);
                    if (password == null)
                        return;
                    pdfDocumentSource.Credential.Password = password;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

        }
    }
}
