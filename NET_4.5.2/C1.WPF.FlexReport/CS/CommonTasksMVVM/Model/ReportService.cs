using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonTasksMVVM.Model
{
    public class ReportService : IReportService
    {
        public void GetCategoryList(Action<CategoryList, Exception> callback)
        {
            var asm = Assembly.GetExecutingAssembly();
            var stream = asm.GetManifestResourceStream("CommonTasksMVVM.Resources.FlexCommonTasks_WPF.flxr");
            CategoryList list = null;
            Exception ex = null;
            try
            {
                list = new CategoryList(stream);
            }
            catch (Exception ex1)
            {
                ex = ex1;
            }
            callback(list, ex);
        }
    }
}
