using C1.FlexPivot;
using C1.PivotEngine;
using Localization.DataSource;
using Localization.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Localization.ViewModel
{
    /// <summary>
    /// ViewModel for managing FlexPivotChart data and engine configuration.
    /// Handles loading mock data and setting up pivot fields for analysis.
    /// </summary>
    public class FlexPivotChartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PersonModel> _data;
        private C1PivotEngine _pivotEngine;

        /// <summary>
        /// Initializes the ViewModel by loading sample data and configuring the PivotEngine.
        /// </summary>
        public FlexPivotChartViewModel()
        {
            LoadData();
        }

        /// <summary>
        /// Gets or sets the collection of person data used as the data source for the pivot chart.
        /// </summary>
        public ObservableCollection<PersonModel> Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Loads sample person data from the PersonFactory.
        /// </summary>
        private void LoadData()
        {
            var dataSource = new PersonFactory();
            Data = new ObservableCollection<PersonModel>(dataSource.Generate(1000));
        }

        /// <summary>
        /// Raised when a property value changes, notifying the UI for data binding updates.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propName">Name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}