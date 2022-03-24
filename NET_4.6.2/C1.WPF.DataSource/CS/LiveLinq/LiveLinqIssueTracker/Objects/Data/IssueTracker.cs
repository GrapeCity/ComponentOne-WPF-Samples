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
using C1.LiveLinq.Collections;
using C1.LiveLinq.Indexing;

namespace LiveLinqIssueTrackerObjects.Data
{
    public class ProductItem
    {
        public virtual int ProductID { get; set; }
        public virtual string ProductName { get; set; }
    }

    public class FeatureItem
    {
        public virtual int ProductID { get; set; }
        public virtual int FeatureID { get; set; }
        public virtual string FeatureName { get; set; }
    }

    public class EmployeeItem
    {
        public virtual int EmployeeID { get; set; }
        public virtual string FullName { get; set; }
    }

    public class AssignmentItem
    {
        public virtual int EmployeeID { get; set; }
        public virtual int ProductID { get; set; }
        public virtual int FeatureID { get; set; }
    }

    public class IssueItem : IndexableObject
    {
        public virtual int IssueID { get; set; }
        public virtual int ProductID { get; set; }
        public virtual int FeatureID { get; set; }
        public virtual string Description { get; set; }

        // Every property of every class above should be implemented like this, but for the purposes of this demo 
        // we only need change notifications in this single property, because we never set other properties
        int assignedTo;
        public int AssignedTo
        { get { return assignedTo; } set { assignedTo = value; OnPropertyChanged("AssignedTo"); } }

        public bool Fixed { get; set; }
    }

    public class IssueTracker
    {
        public IndexedCollection<ProductItem> Products = new IndexedCollection<ProductItem>();
        public IndexedCollection<FeatureItem> Features = new IndexedCollection<FeatureItem>();
        public IndexedCollection<IssueItem> Issues = new IndexedCollection<IssueItem>();
        public IndexedCollection<EmployeeItem> Employees = new IndexedCollection<EmployeeItem>();
        public IndexedCollection<AssignmentItem> Assignments = new IndexedCollection<AssignmentItem>();

        Index<IssueItem> _issuesById;
        Index<AssignmentItem> _assignmentsByEmployeeId;

        public IssueTracker()
        {
            var doc = new XDocument();
            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LiveLinqIssueTrackerObjects.Data.IssueTracking.xml"))
            {
                Debug.Assert(xmlStream != null);
                XmlReaderSettings settings = new XmlReaderSettings { IgnoreWhitespace = true };
                XmlReader reader = XmlReader.Create(xmlStream, settings);
                XElement data = XElement.Load(reader);
                doc.ReplaceNodes(data);
            }
            
            foreach (var x in doc.AsLive().Root().Elements("Employees"))
                Employees.Add(new EmployeeItem
                        {
                            EmployeeID = (int)x.Element("EmployeeID"),
                            FullName = (string)x.Element("FullName")
                        });

            foreach (var x in doc.AsLive().Root().Elements("Features"))
                Features.Add(new FeatureItem
                       {
                           ProductID = (int)x.Element("ProductID"),
                           FeatureID = (int)x.Element("FeatureID"),
                           FeatureName = (string)x.Element("FeatureName")
                       });

            foreach (var x in doc.AsLive().Root().Elements("Products"))
                Products.Add(new ProductItem
                          {
                              ProductID = (int)x.Element("ProductID"),
                              ProductName = (string)x.Element("ProductName"),
                          });

            foreach (var x in doc.AsLive().Root().Elements("Issues"))
                Issues.Add(new IssueItem
                     {
                         IssueID = (int)x.Element("IssueID"),
                         ProductID = (int)x.Element("ProductID"),
                         FeatureID = (int)x.Element("FeatureID"),
                         Description = (string)x.Element("Description"),
                         AssignedTo = (int)x.Element("AssignedTo"),
                         Fixed = (bool)x.Element("Fixed")
                     });

            foreach (var x in doc.AsLive().Root().Elements("Assignments"))
                Assignments.Add(new AssignmentItem
                     {
                         EmployeeID = (int)x.Element("EmployeeID"),
                         ProductID = (int)x.Element("ProductID"),
                         FeatureID = (int)x.Element("FeatureID"),
                     });

            _issuesById = Issues.Indexes.Add(i => i.IssueID, true);
            _assignmentsByEmployeeId = Assignments.Indexes.Add(a => a.EmployeeID);
        }

        public IssueItem FindIssueByID(int issueID)
        {
            return _issuesById.Find(issueID).FirstOrDefault();
        }

        public IEnumerable<AssignmentItem> GetAssignmentsByEmployeeID(int employeeID)
        {
            return _assignmentsByEmployeeId.Find(employeeID);
        }    
    }
}
