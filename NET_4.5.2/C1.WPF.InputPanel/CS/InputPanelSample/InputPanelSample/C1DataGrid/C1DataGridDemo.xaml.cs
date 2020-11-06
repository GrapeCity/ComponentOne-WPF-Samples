using C1.WPF;
using C1.WPF.InputPanel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.DataGrid;
using System.Reflection;
using System.Globalization;

namespace InputPanelSample
{
    /// <summary>
    /// Interaction logic for C1DataGrid.xaml
    /// </summary>
    public partial class C1DataGridDemo : UserControl
    {
        public C1DataGridDemo()
        {
            InitializeComponent();
            DataContext = new Data();
            InputPanel.ValidateCurrentItem += InputPanel_ValidateCurrentItem;
        }

        private void InputPanel_ValidateCurrentItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
            C1InputPanel inputPanel = sender as C1InputPanel;
            DataRowView view = inputPanel.CurrentItem as DataRowView;
            if (view != null)
            {
                var row = view.Row;
                var errorList = new ObservableCollection<ErrorInfo>();
                if (row["Last"] != null && string.IsNullOrWhiteSpace(row["Last"].ToString()))
                {
                    errorList.Add(new ErrorInfo { ErrorInputName = "Last", ErrorContent = "Last name is required." });
                }
                if (row["Last"] != null && row["Last"].ToString().Length > 15)
                {
                    errorList.Add(new ErrorInfo { ErrorInputName = "Last", ErrorContent = "Last Name should be less than 15 characters." });
                }
                if (row["Weight"] != null && row["Weight"].ToString().Length > 0 && (double)row["Weight"] > 10000)
                {
                    errorList.Add(new ErrorInfo { ErrorInputName = "Weight", ErrorContent = "Weight is too high." });
                }
                if (row["BirthDate"] != null && !string.IsNullOrWhiteSpace(row["BirthDate"].ToString()) && (DateTime)row["BirthDate"] > DateTime.Now)
                {
                    errorList.Add(new ErrorInfo { ErrorInputName = "BirthDate", ErrorContent = "The birth date should not late than now." });
                }
                if (row["Phone"] != null && string.IsNullOrEmpty(row["Phone"].ToString().Replace("\0", "")))
                {
                    errorList.Add(new ErrorInfo { ErrorInputName = "Phone", ErrorContent = "Phone number is required." });
                }
                inputPanel.ValidationErrors = errorList;
                if (errorList.Count > 0)
                {
                    e.Cancel = true;
                }
            }
        }

        private void C1DataGrid_AutoGeneratingColumn(object sender, C1.WPF.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Property.Name == "Occupation")
            {              
                var comboCol = e.Column as C1.WPF.DataGrid.DataGridComboBoxColumn;
                if (comboCol != null && comboCol.Binding != null)
                {
                    comboCol.Binding.Converter = new EnumConverter(e.Property.PropertyType);
                }            
            }
        }
    }

    internal class EnumConverter : IValueConverter
    {
        private Type _enumType;
        public EnumConverter(Type enumType)
        {
            _enumType = enumType;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (_enumType == null)
                {
                    _enumType = value.GetType();
                }
                try
                {
                    bool isEnum = _enumType.IsEnum;

                    if (!isEnum)
                    {
                        return value;
                    }
                    return Enum.Parse(_enumType, value.ToString());
                }
                catch { };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (_enumType == null)
                {
                    _enumType = value.GetType();
                }
                try
                {
                    bool isEnum = _enumType.IsEnum;

                    if (!isEnum)
                    {
                        return value;
                    }
                    return Enum.Parse(_enumType, value.ToString());
                }
                catch { };
            }
            return null;
        }        
    }
}