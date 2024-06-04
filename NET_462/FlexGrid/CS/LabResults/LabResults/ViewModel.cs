using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace LabResults
{
    public class ViewModel
    {
        CollectionViewSource _results = new CollectionViewSource();
        CollectionViewSource _patients = new CollectionViewSource();

        /// <summary>
        /// Initializes a new instance of a <see cref="ViewModel"/>.
        /// </summary>
        public ViewModel()
        {
            var results = TestResult.GetResults(300);

            // create patient list
            var q = from tr in results orderby tr.Patient select tr.Patient;
            _patients.Source = q.Distinct().ToList();

            // create test result view (grouped by date, filtered by current patient)
            _results.Source = results;
            _results.View.GroupDescriptions.Add(new PropertyGroupDescription("DatePosted"));
            _results.View.Filter = Filter;

            // select the first patient
            Patients.CurrentChanged += (s, e) => { TestResults.Refresh(); };
            Patients.MoveCurrentToFirst();
        }
        /// <summary>
        /// Gets the filtered, grouped view of the test results.
        /// </summary>
        public ICollectionView TestResults
        {
            get { return _results.View; }
        }
        /// <summary>
        /// Gets a list of unique patient names.
        /// </summary>
        public ICollectionView Patients
        {
            get { return _patients.View; }
        }
        /// <summary>
        /// Gets or sets the currently selected patient.
        /// </summary>
        public string SelectedPatient
        {
            get { return (string)Patients.CurrentItem; }
            set { Patients.MoveCurrentTo((string)value); }
        }
        /// <summary>
        /// Filter the result view to show only the current patient.
        /// </summary>
        bool Filter(object item)
        {
            var tr = item as TestResult;
            return string.IsNullOrEmpty(SelectedPatient) || tr.Patient == SelectedPatient;
        }
    }
}
