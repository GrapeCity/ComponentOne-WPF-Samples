using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using C1.WPF.RichTextBox.Documents;
using C1.WPF.RichTextBox;
using System.Reflection;

namespace RichTextBoxExplorer
{
    public partial class DemoRichTextBox : UserControl
    {
        private string _asmName = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name;
        public event EventHandler<RequestNavigateEventArgs> RequestNavigate;

        public DemoRichTextBox()
        {
            InitializeComponent();
            Tag = Properties.Resources.Demo;

            //var spellChecker = new C1SpellChecker();
            //richTB.SpellChecker = spellChecker;
            //spellChecker.MainDictionary.Load(Application.GetResourceStream(new Uri("/" + _asmName + ";component/Resources/C1Spell_en-US.dct", UriKind.Relative)).Stream);

            var stream = Application.GetResourceStream(new Uri("/" + _asmName + ";component/Resources/dickens.htm", UriKind.Relative)).Stream;
            var html = new StreamReader(stream).ReadToEnd();
            richTB.HtmlFilter.ConvertingHtmlNode += OnConvertingHtmlNode;
            richTB.Html = html;
            richTB.NavigationMode = NavigationMode.Always;
            richTB.RequestNavigate += OnRequestNavigate;

            //ribbon.IsCollapsed = false;
            //ribbon.SelectedTabIndex = 1;
        }

        void OnConvertingHtmlNode(object sender, ConvertingHtmlNodeEventArgs e)
        {
            var element = e.HtmlNode as C1HtmlElement;
            if (element != null && element.Name == "img")
            {
                string src;
                if (element.Attributes.TryGetValue("src", out src) && src.StartsWith("images"))
                {
                    element.Attributes["src"] = "/" + _asmName + ";component/Resources/" + src;
                }
            }
        }

        void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (RequestNavigate != null)
            {
                RequestNavigate(sender, e);
            }

            var uri = e.Hyperlink.NavigateUri != null ? e.Hyperlink.NavigateUri.ToString() : "";
            if (uri.StartsWith("#") && uri.Length > 1)
            {
                var id = uri.Substring(1);
                var element = richTB.Document.EnumerateSubtree().FirstOrDefault(el => el.Id == id);
                if (element != null)
                {
                    richTB.ScrollTo(element.ContentStart.ClosestCaret);
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return richTB.IsReadOnly;
            }
            set
            {
                richTB.IsReadOnly = value;
            }
        }

        public bool AcceptsReturn
        {
            get
            {
                return richTB.AcceptsReturn;
            }
            set
            {
                richTB.AcceptsReturn = value;
            }
        }

        public NavigationMode NavigationMode
        {
            get
            {
                return richTB.NavigationMode;
            }
            set
            {
                richTB.NavigationMode = value;
            }
        }

        public Brush SelectionBackground
        {
            get { return richTB.SelectionBackground; }
            set { richTB.SelectionBackground = value; }
        }

        public Brush SelectionForeground
        {
            get { return richTB.SelectionForeground; }
            set { richTB.SelectionForeground = value; }
        }

    }
}
