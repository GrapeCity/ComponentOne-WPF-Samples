using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using CommonTasksMVVM.Model;

using C1.WPF.FlexReport;

namespace CommonTasksMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        C1FlexReport _flexReport;
        CategoryList _categoryList;
        string _errorMessage;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IReportService reportService)
        {
            reportService.GetCategoryList(
                (list, error) =>
                {
                    if (error != null)
                    {
                        ErrorMessage = error.Message;
                        return;
                    }

                    CategoryList = list;
                    FlexReport = list.GetReport(list[0].Reports[0]);
                });

            ClickReportCommand = new RelayCommand<string>(ClickReportMethod);

            Messenger.Default.Register<CleanupMsg>(this, (cm) => Cleanup());
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            private set
            {
                Set(() => ErrorMessage, ref _errorMessage, value);
            }
        }

        public CategoryList CategoryList
        {
            get
            {
                return _categoryList;
            }
            private set
            {
                Set(() => CategoryList, ref _categoryList, value);
            }
        }

        public ICommand ClickReportCommand { get; private set; }

        public void ClickReportMethod(string reportName)
        {
            if (_categoryList != null)
            {
                var oldRpt = _flexReport;
                FlexReport = _categoryList.GetReport(reportName);
                if (oldRpt != null)
                {
                    oldRpt.Dispose();
                }
            }
        }

        public C1FlexReport FlexReport
        {
            get
            {
                return _flexReport;
            }
            private set
            {
                Set(() => FlexReport, ref _flexReport, value);
            }
        }

        public override void Cleanup()
        {
            var rpt = _flexReport;
            if (rpt != null)
            {
                FlexReport = null;
                rpt.Dispose();
            }

            var list = _categoryList;
            if (list != null)
            {
                CategoryList = null;
                list.Dispose();
            }

            base.Cleanup();
        }

        public class CleanupMsg : MessageBase
        {
        }
    }
}
