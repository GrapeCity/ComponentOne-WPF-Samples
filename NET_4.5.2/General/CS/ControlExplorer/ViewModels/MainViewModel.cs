using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using ControlExplorer.Common;
using System.Collections.Generic;
using System.Globalization;

namespace ControlExplorer
{
    public class MainViewModel
    {
        private static MainViewModel _instance = null;

        private MainViewModel()
        {
            LoadGroups();
            ExpandedCollpasedCommand = new RelayCommand(new Action<object>(e => 
            {
                if (Groups != null)
                {
                    bool parameter;
                    if (bool.TryParse(e.ToString(), out parameter))
                    {
                        Groups.ToList().ForEach(g => g.IsExpanded = parameter);
                    }
                }
            }));
        }

        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainViewModel();
                return _instance;
            }
        }

        public IEnumerable<GroupDescription> Groups
        {
            get; private set;
        }

        public IEnumerable<ControlDescription> Controls
        {
            get; private set;
        }

        public IEnumerable<ControlDescription> TopControls
        {
            get; private set;
        }

        public IEnumerable<ControlDescription> NewControls
        {
            get; private set;
        }

        public IEnumerable<FeatureDescription> Features
        {
            get
            {
                return Controls.SelectMany(c => c.GetAllFeatures());
            }
        }

        public RelayCommand ExpandedCollpasedCommand
        {
            get;private set;
        }

        private void LoadGroups()
        {
            string ci = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            string resourceFile = ci == "en" ? "ControlList" : ci + ".ControlList";

            XDocument doc = null;
            try
            {
                doc = LoadXmlResource($"ControlExplorer.Resources.{resourceFile}.xml");
            }
            catch
            {
                doc = LoadXmlResource($"ControlExplorer.Resources.ControlList.xml"); //fallback to EN if exception occurs
            }
            Groups = (from g in doc.Root.Elements("Group")
                         select new GroupDescription(g)).ToList();

            Controls = from g in Groups
                       from c in g.Controls
                       orderby c.Name ascending
                       select c;

            NewControls = from c in Controls
                          where c.IsNew
                          select c;

            TopControls = from c in Controls
                          where c.IsTop
                          select c;
        }

        private static XDocument LoadXmlResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return XDocument.Load(XmlReader.Create(assembly.GetManifestResourceStream(resourceName)));
        }
    }
}
