using Localization.DataSource;
using Localization.Model;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Localization.ViewModel
{
    /// <summary>
    /// ViewModel for managing data displayed in the FlexGrid tab.
    /// Responsible for generating and binding a large dataset of PersonModel objects.
    /// </summary>
    public class FlexGridTabViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<PersonModel> _people;
        private readonly PersonFactory _generatePeople;

        /// <summary>
        /// A collection of randomly generated PersonModel data entries used by the FlexGrid.
        /// </summary>
        public ObservableCollection<PersonModel> People
        {
            get { return _people; }
            set
            {
                if (_people != value)
                {
                    _people = value;
                    OnPropertyChanged(nameof(People));
                }
            }
        }


        /// <summary>
        /// Initializes the ViewModel by creating a PersonFactory instance and loading mock data.
        /// </summary>
        public FlexGridTabViewModel()
        {
            _generatePeople = new PersonFactory();
            LoadPeople();
        }

        /// <summary>
        /// Populates the People collection with a large sample dataset of PersonModel entries.
        /// </summary>
        private void LoadPeople()
        {
            var list = _generatePeople.Generate(1000000);
            People = new ObservableCollection<PersonModel>(list);
        }


        /// <summary>
        /// Raised whenever a property value changes to notify the UI for binding updates.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Invokes the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}