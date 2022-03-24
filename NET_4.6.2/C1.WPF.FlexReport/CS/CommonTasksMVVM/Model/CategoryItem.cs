using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTasksMVVM.Model
{
    public class CategoryItem
    {
        List<string> _reports;

        public CategoryItem(string categoryName)
        {
            CategoryName = categoryName;
            _reports = new List<string>();
        }

        public string CategoryName
        {
            get;
            private set;
        }

        public List<string> Reports
        {
            get { return _reports; }
        }

        public void AddReport(string reportName)
        {
            _reports.Add(reportName);
        }
    }
}
