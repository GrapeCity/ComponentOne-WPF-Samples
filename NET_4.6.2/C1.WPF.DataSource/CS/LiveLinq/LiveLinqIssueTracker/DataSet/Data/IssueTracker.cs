using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Data;

namespace LiveLinqIssueTrackerDataSet.Data
{
    public class IssueTracker
    {
        public IssueTracking DataSet { private set; get; }
        public XDocument XmlDocument { private set; get; }

        public IssueTracker()
        {
            DataSet = new IssueTracking();
            DataSet.DataSetName = "IssueTracking";
            XmlDocument = new XDocument();
            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveLinqIssueTrackerDataSet.Data.IssueTracking.xml"))
            {
                Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                XElement data = XElement.Load(reader);
                XmlDocument.ReplaceNodes(data);
                DataSet.ReadXml(XmlDocument.CreateReader());
            }
        }
    }
}
