using C1.Chart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;

namespace StockChart.ViewModel
{

    public class ExportCommand : DependencyObject, System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, new EventArgs());
            }
        }

        ChartViewModel _viewModel;
        public ExportCommand(ChartViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public void Execute(object parameter)
        {
            string type = parameter.ToString();


            string filter = string.Empty;
            C1.WPF.Chart.ImageFormat format = C1.WPF.Chart.ImageFormat.Png;
            switch (type)
            {
                case "PNG":
                    filter = "PNG|*.png";
                    format = C1.WPF.Chart.ImageFormat.Png;
                    break;
                case "JPG":
                    filter = "JPG|*.jpg";
                    format = C1.WPF.Chart.ImageFormat.Jpeg;
                    break;
                case "SVG":
                    filter = "SVG|*.svg";
                    format = C1.WPF.Chart.ImageFormat.Svg;
                    break;
                default:
                    filter = "PNG|*.png";
                    format = C1.WPF.Chart.ImageFormat.Png;
                    break;
            }


            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = filter;
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                using (System.IO.FileStream fs = new System.IO.FileStream(dialog.FileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                {
                    _viewModel.MainChart.SaveImage(fs, format, (int)_viewModel.MainChart.ActualWidth, (int)_viewModel.MainChart.ActualHeight);
                }
            }

        }

    }
}
