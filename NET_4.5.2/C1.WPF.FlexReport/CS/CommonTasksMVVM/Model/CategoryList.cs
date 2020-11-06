using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

using C1.WPF.FlexReport;

namespace CommonTasksMVVM.Model
{
    public class CategoryList : KeyedCollection<string, CategoryItem>, IDisposable
    {
        Stream _stream;

        public CategoryList(Stream stream)
        {
            _stream = stream;
            string[] reports = C1FlexReport.GetReportList(stream);

            using (C1FlexReport fr = new C1FlexReport())
            {
                foreach (string reportName in reports)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    fr.Load(stream, reportName);

                    string keywords = fr.ReportInfo.Keywords;
                    string[] ss = keywords.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    for (int i = 0; i < ss.Length; i++)
                    {
                        var categoryName = ss[i];

                        if (Contains(categoryName))
                            this[categoryName].AddReport(reportName);
                        else
                        {
                            var ci = new CategoryItem(categoryName);
                            ci.AddReport(reportName);
                            this.Add(ci);
                        }
                    }
                }
            }
        }

        public C1FlexReport GetReport(string reportName)
        {
            if (_stream != null)
            {
                _stream.Seek(0, SeekOrigin.Begin);
                var fr = new C1FlexReport();
                fr.Load(_stream, reportName);
                return fr;
            }
            return null;
        }

        protected override string GetKeyForItem(CategoryItem item)
        {
            return item.CategoryName;
        }

        public void Dispose()
        {
            _stream.Close();
            _stream = null;
        }
    }
}
