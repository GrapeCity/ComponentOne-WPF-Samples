using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Diagnostics;
using C1.LiveLinq.LiveViews;
using C1.LiveLinq.LiveViews.Xml;
using C1.LiveLinq.Indexing;

namespace LiveLinqIssueTrackerXML.Data
{
    public class Product
    {
        public virtual int ProductID { get; set; }
        public virtual string ProductName { get; set; }
        public virtual IList<Feature> Features { get; set; }
    }

    public class Feature
    {
        public virtual int ProductID { get; set; }
        public virtual int FeatureID { get; set; }
        public virtual string FeatureName { get; set; }
    }

    public class Employee
    {
        public virtual int EmployeeID { get; set; }
        public virtual string FullName { get; set; }
    }

    public class Assignment
    {
        public virtual int EmployeeID { get; set; }
        public virtual int ProductID { get; set; }
        public virtual int FeatureID { get; set; }
    }

    public class Issue
    {
        public virtual int IssueID { get; set; }
        public virtual int ProductID { get; set; }
        public virtual int FeatureID { get; set; }
        public virtual string Description { get; set; }
        public virtual int AssignedTo { get; set; }
        public virtual bool Fixed { get; set; }
    }

    public class IssueTracker
    {
        public View<Product> Products { get; private set; }
        public View<Feature> Features { get; private set; }
        public View<Issue> Issues { get; private set; }
        public View<Employee> Employees { get; private set; }
        public View<Assignment> Assignments { get; private set; }
        public XDocument Document { get; private set; }

        Index<ViewRow> _issuesById;

        public IssueTracker()
        {
            Document = new XDocument();
            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveLinqIssueTrackerXML.Data.IssueTracking.xml"))
            {
                Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                XElement data = XElement.Load(reader);
                Document.ReplaceNodes(data);
            }

            Employees = from x in Document.AsLive().Root().Elements("Employees")
                        select new Employee
                        {
                            EmployeeID = (int)x.Element("EmployeeID"),
                            FullName = (string)x.Element("FullName")
                        };

            Features = from x in Document.AsLive().Root().Elements("Features")
                       select new Feature
                       {
                           ProductID = (int)x.Element("ProductID"),
                           FeatureID = (int)x.Element("FeatureID"),
                           FeatureName = (string)x.Element("FeatureName")
                       };

            Products = from x in Document.AsLive().Root().Elements("Products")
                       join f in Features on (int)x.Element("ProductID") equals f.ProductID into g
                       select new Product
                       {
                           ProductID = (int)x.Element("ProductID"),
                           ProductName = (string)x.Element("ProductName"),
                           Features = g
                       };

            Issues = from x in Document.AsLive().Root().Elements("Issues")
                     select new Issue
                     {
                         IssueID = (int)x.Element("IssueID"),
                         ProductID = (int)x.Element("ProductID"),
                         FeatureID = (int)x.Element("FeatureID"),
                         Description = (string)x.Element("Description"),
                         AssignedTo = (int)x.Element("AssignedTo"),
                         Fixed = (bool)x.Element("Fixed")
                     };

            _issuesById = Issues.Rows.Indexes.Add(row => ((Issue)row.Value).IssueID, true);

            Assignments = from x in Document.AsLive().Root().Elements("Assignments")
                          select new Assignment
                          {
                              EmployeeID = (int)x.Element("EmployeeID"),
                              ProductID = (int)x.Element("ProductID"),
                              FeatureID = (int)x.Element("FeatureID"),
                          };
        }

        public ViewRow FindIssueByID(int issueID)
        {
            return _issuesById.Find(issueID).FirstOrDefault();
        }
    }
}
