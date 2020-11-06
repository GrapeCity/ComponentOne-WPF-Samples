using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LabResults
{
    /// <summary>
    /// TestResult is our main (only) data item.
    /// </summary>
    public class TestResult : BaseObject
    {
        string _patient, _testName, _testResult;
        DateTime _datePosted;
        bool _corrected;

        [Display(Name = "Patient")]
        public string Patient
        {
            get { return _patient; }
            set
            {
                if (value != _patient)
                {
                    _patient = value;
                    OnPropertyChanged("Patient");
                }
            }
        }

        [Display(Name = "TestName")]
        public string TestName
        {
            get { return _testName; }
            set
            {
                if (value != _testName)
                {
                    _testName = value;
                    OnPropertyChanged("TestName");
                }
            }
        }

        [Display(Name = "Result")]
        public string Result
        {
            get { return _testResult; }
            set
            {
                if (value != _testResult)
                {
                    _testResult = value;
                    OnPropertyChanged("TestResult");
                }
            }
        }

        [Display(Name = "DatePosted")]
        public DateTime DatePosted
        {
            get { return _datePosted; }
            set
            {
                if (value != _datePosted)
                {
                    _datePosted = value;
                    OnPropertyChanged("DatePosted");
                }
            }
        }

        [Display(Name = "Corrected")]
        public bool Corrected
        {
            get { return _corrected; }
            set
            {
                if (value != _corrected)
                {
                    _corrected = value;
                    OnPropertyChanged("Corrected");
                }
            }
        }

        /// <summary>
        /// Creates an observable collection of TestResult objects for use in testing.
        /// </summary>
        public static ObservableCollection<TestResult> GetResults(int count)
        {
            var first = "Mark|Susan|Dean|Chris|Pat".Split('|');
            var last = "Smith|Doe|Paulson|Minelli".Split('|');
            var test = "Methadone|Opiate|Cocaine|Benzos|Alcohol|Creatinine".Split('|');
            var rnd = new Random(0);

            var list = new ObservableCollection<TestResult>();
            for (int i = 0; i < count; i++)
            {
                var result = new TestResult();
                result.Patient = first[rnd.Next() % first.Length] + " " + last[rnd.Next() % last.Length];
                result.TestName = test[rnd.Next() % test.Length];
                result.Result = rnd.NextDouble() > .5 ? "Positive" : "Negative";
                result.DatePosted = DateTime.Today.AddDays(rnd.Next(-7, -4));
                result.Corrected = rnd.NextDouble() > .5;
                list.Add(result);
            }
            return list;
        }
    }
}
