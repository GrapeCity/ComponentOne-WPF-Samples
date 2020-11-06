using C1.WPF.RichTextBox.Documents;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace RichTextBoxExplorer
{
    public partial class ToolStrip : UserControl
    {
        private string _asmName = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name;

        public ToolStrip()
        {
            InitializeComponent();
            Tag = "This sample shows C1RichTextBox with a simpler editing toolstrip.";

            var stream = Application.GetResourceStream(new Uri("/" + _asmName + ";component/Resources/letter.rtf", UriKind.Relative)).Stream;
            var rtf = new StreamReader(stream).ReadToEnd();
            var filter = new RtfFilter();
            richTB.Document = filter.ConvertToDocument(rtf);
        }
    }
}
