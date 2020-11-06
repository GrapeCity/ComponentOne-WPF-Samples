using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Reflection;
using System.ComponentModel;
using C1.WPF.Extended;
using System.Globalization;
using ControlExplorer.Common;

namespace ControlExplorer
{
    public class FeatureDescription : BindableBase, ISearchable
    {
        const string EditingControlPropertyName = "C1ControlExplorerSampleEditingControl";
        
        private string _name;
        private string _description;
        private bool _isExpanded;
        private Uri _packageUri;
        private string _assemblyName;
        private FrameworkElement _sample;
        private FrameworkElement _editingControl = null;
        
        public FeatureDescription() { }
        // constructor
        public FeatureDescription(ControlDescription control, FeatureDescription parent, XElement f)
        {
            bool b = false;
            Control = control;
            Description = (f.Element("Description") != null) ? f.Element("Description").Value : string.Empty;
            Name = (f.Attribute("name") != null) ? f.Attribute("name").Value : string.Empty;
            IsNew = (f.Attribute("isNew") != null && bool.TryParse(f.Attribute("isNew").Value, out b) ? b : false);
            IsExpanded = (f.Attribute("isExpanded") != null) ? bool.Parse(f.Attribute("isExpanded").Value) : false;
            AssemblyName = (f.Attribute("assemblyName") != null) ? f.Attribute("assemblyName").Value : (f.Attribute("type") != null ? f.Attribute("type").Value.Split('.')[0] + ".4.dll" : string.Empty);
            PackageUri = (f.Attribute("packageUri") != null) ? new Uri(f.Attribute("packageUri").Value, UriKind.RelativeOrAbsolute) : null;
            DemoControlTypeName = (f.Attribute("type") != null) ? f.Attribute("type").Value : string.Empty;
            Source = (f.Attribute("source") != null) ? f.Attribute("source").Value : string.Empty;
            Event = (f.Element("Event") != null ? f.Element("Event").Value : "");
            OwnerFeature = parent;
            Link = new Uri(("/" + control.Name + (OwnerFeature != null ? ("/" + OwnerFeature.Name) : "") + "/" + Name).Trim().Replace("?", "").Replace("'", "").Replace("(", "").Replace(")", ""), UriKind.Relative);
            if (f.Element("SubFeatures") != null)
            {
                SubFeatures = (from sf in f.Element("SubFeatures").Elements("SubFeature") select new FeatureDescription(control, this, sf)).ToList();
            }
            else
            {
                SubFeatures = new List<FeatureDescription>();
            }
            if (f.Element("Properties") != null)
            {
                var properties = from pair in f.Element("Properties").Elements("Property")
                                 select new PropertyAttribute
                                 {
                                     MemberName = pair.Attribute("name").Value,
                                     DisplayName = (pair.Attribute("caption") != null ? pair.Attribute("caption").Value : pair.Attribute("name").Value),
                                     DefaultValue = (pair.Attribute("value") != null ? pair.Attribute("value").Value : null),
                                     Browsable = (pair.Attribute("display") != null ? bool.Parse(pair.Attribute("display").Value) : true),
                                     MinimumValue = (pair.Attribute("minimumValue") != null ? double.Parse(pair.Attribute("minimumValue").Value, CultureInfo.InvariantCulture) : double.NaN),
                                     MaximumValue = (pair.Attribute("maximumValue") != null ? double.Parse(pair.Attribute("maximumValue").Value, CultureInfo.InvariantCulture) : double.NaN),
                                     Tag = (pair.Attribute("nullable") != null ? bool.Parse(pair.Attribute("nullable").Value) : false),
                                 };
                Properties = new List<PropertyAttribute>(properties);
            }
        }

        public DataTemplate Icon
        {
            get
            {
                // update Icon
                switch (Type)
                {
                    case FeatureType.RelatedSamples:
                        return (DataTemplate)Application.Current.Resources["IconRelatedSamples"];

                    case FeatureType.Default:
                    case FeatureType.Custom:
                    default:
                        if (SubFeatures.Count() == 0)
                        {
                            return (DataTemplate)Application.Current.Resources["IconFeatures"];
                        }
                        else
                        {
                            return (DataTemplate)Application.Current.Resources["IconFolder"];
                        }
                }
            }
        }
        public int ID { get; set; }
        // general information
        public string Name
        {
            get
            {
                if (Type == FeatureType.Custom)
                {
                    return _name;
                }
                else
                {
                    return (string.IsNullOrEmpty(_name) ? "See it in action" : _name);
                }
            }
            set
            {
                _name = value;
            }
        }
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(_description))
                {
                    if (OwnerFeature != null)
                    {
                        return OwnerFeature.Description;
                    }
                    if (Control != null)
                    {
                        return Control.Description;
                    }
                }
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public Uri Link { get; set; }

        public string Source { get; set; }

        public FrameworkElement Sample
        {
            get
            {
                return _sample;
            }
            set
            {
                _sample = value;
                if (Properties != null)
                {
                    foreach (var p in Properties)
                    {
                        if (p.MemberName == "VerticalAlignment")
                        {
                            _sample.VerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), p.DefaultValue as string, false);
                        }
                        else if (p.MemberName == "HorizontalAlignment")
                        {
                            _sample.HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), p.DefaultValue as string, false);
                        }
                    }
                }
                if (_sample == null)
                    _editingControl = null;
                else
                {
                    PropertyInfo pi;
                    try
                    {
                        pi = _sample.GetType().GetProperty(EditingControlPropertyName);
                    }
                    catch
                    {
                        pi = null;
                    }
                    if (pi != null)
                    {
                        try
                        {
                            _editingControl = pi.GetValue(_sample, null) as FrameworkElement;
                        }
                        catch
                        {
                            _editingControl = _sample;
                        }
                    }
                    else
                        _editingControl = _sample;
                }
                OnPropertyChanged("Sample");
                OnPropertyChanged("EditingControl");
            }
        }
        public FrameworkElement EditingControl
        {
            get { return _editingControl; }
        }
        public FeatureType Type
        {
            get
            {
                //if (Samples.Count > 0)
                //{
                //    return FeatureType.RelatedSamples;
                //}
                //else
                {
                    return FeatureType.Default;
                }
            }
        }

        public bool LocalhostOnly { get; set; }

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                SetProperty(ref _isExpanded, value, "IsExpanded");
            }
        }

        // visual behavior
        public bool ShowAnimation { get; set; }

        // demo
        public string DemoControlTypeName { get; set; }
        public ControlDescription Control { get; set; }

        public bool IsNew { get; set; }

        public string SolutionPath;
        public string XamlPath;

        public string FullDemoControlTypeName
        {
            get
            {
                if (string.IsNullOrEmpty(DemoControlTypeName))
                {
                    return Control.AssemblyName + ".C1" + Control.Name; ;
                }
                else
                {
                    return DemoControlTypeName;
                }
            }
        }

        #region assembly

        public Uri PackageUri
        {
            get
            {
                if (_packageUri != null)
                {
                    return _packageUri;
                }
                if (string.IsNullOrEmpty(DemoControlTypeName))
                {
                    return new Uri(Control.AssemblyName + ".zip", UriKind.RelativeOrAbsolute);
                }
                else
                {
                    return new Uri(AssemblyName + ".xap", UriKind.RelativeOrAbsolute);
                }
            }
            set
            {
                _packageUri = value;
            }
        }

        public string AssemblyName
        {
            get
            {
                if (!string.IsNullOrEmpty(_assemblyName))
                {
                    return _assemblyName;
                }
                if (string.IsNullOrEmpty(DemoControlTypeName))
                {
                    return Control.AssemblyName;
                }
                else
                {
                    return DemoControlTypeName.Substring(0, DemoControlTypeName.IndexOf('.'));
                }
            }
            set
            {
                _assemblyName = value;
            }
        }

        #endregion

        // default type properties
        public List<PropertyAttribute> Properties { get; set; }

        public Visibility PropertiesVisibility
        {
            get
            {
                return Properties != null && Properties.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        //public List<MethodAttribute> Actions { get; set; }
        public string Event { get; set; }

        // related sample properties
        //public List<SampleInformation> Samples { get; set; }

        public IList<FeatureDescription> SubFeatures { get; set; }
        public FeatureDescription OwnerFeature { get; set; }

        #region ISearchable Members

        public bool Contains(string word)
        {
            word = word.ToLower();
            bool inSubFeature = false;
            foreach (var sf in SubFeatures)
            {
                inSubFeature |= sf.Contains(word);
            }

            return inSubFeature || Name.ToLower().Contains(word);
        }

        public bool ContainsAny(string[] searchKeys)
        {
            return searchKeys.Any(key => Contains(key));
        }

        #endregion
    }

    public enum FeatureType
    {
        Default,
        Custom,
        RelatedSamples
    }
}
