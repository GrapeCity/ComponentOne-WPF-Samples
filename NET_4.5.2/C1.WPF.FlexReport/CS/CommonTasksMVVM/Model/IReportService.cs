using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using C1.WPF.FlexReport;

namespace CommonTasksMVVM.Model
{
    public interface IReportService
    {
        void GetCategoryList(Action<CategoryList, Exception> callback);
    }
}
