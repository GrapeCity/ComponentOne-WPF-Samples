using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Xml.Linq;
using ControlExplorer.Common;

namespace ControlExplorer
{
    public class ControlDescription: BindableBase, ISearchable
    {
        public ControlDescription(XElement c)
        {
            bool b = false;
            IsNew = (c.Attribute("isNew") != null && bool.TryParse(c.Attribute("isNew").Value, out b) ? b : false);
            IsTop = (c.Attribute("isTop") != null && bool.TryParse(c.Attribute("isTop").Value, out b) ? b : false);
            AssemblyName = c.Attribute("assembly") != null ? c.Attribute("assembly").Value : string.Empty;
            Name = c.Attribute("name").Value;
            Description = (c.Element("Description") != null) ? c.Element("Description").Value : string.Empty;
            Features = (from f in c.Elements("Feature") select new FeatureDescription(this, null, f)).ToList();
        }

        public bool IsNew { get; set; }

        public bool IsTop { get; set; }

        public string Name { get; set; }

        public string AssemblyName { get; set; }

        public Uri Link
        {
            get
            {
                var newF = Features.FirstOrDefault(c => c.IsNew || c.IsExpanded);
                var first = newF ?? Features.FirstOrDefault();
                if (first != null)
                    return first.Link;
                else
                    return null;
            }
        }

        public string Description { get; set; }

        public DataTemplate Icon
        {
            get
            {
                return (DataTemplate)Application.Current.Resources["IconC1" + Name.Replace(" ", "")] ?? (DataTemplate)Application.Current.Resources["IconC1Gray"];
            }
        }

        public DataTemplate IconSearch
        {
            get
            {
                return (DataTemplate)Application.Current.Resources["IconSearchC1" + Name.Replace(" ", "")];
            }
        }
        public IList<FeatureDescription> Features { get; set; }

        #region ISearchable Members

        public bool Contains(string word)
        {
            return Name.ToLower().Contains(word.ToLower());
        }

        public bool ContainsAny(string[] searchKeys)
        {
            return searchKeys.Any(key => Contains(key));
        }

        #endregion

        internal IEnumerable<FeatureDescription> GetAllFeatures()
        {
            return Features.SelectMany(f => f.SubFeatures.Count() > 0 ? f.SubFeatures.ToList().Concat(new List<FeatureDescription> {f}) : new List<FeatureDescription> {f});
        }
    }
}
