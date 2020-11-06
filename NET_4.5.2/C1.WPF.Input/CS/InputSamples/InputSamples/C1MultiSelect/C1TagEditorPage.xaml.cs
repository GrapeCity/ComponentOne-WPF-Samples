using C1.WPF.Input;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InputSamples
{
    /// <summary>
    /// Interaction logic for C1TagEditorPage.xaml
    /// </summary>
    public partial class C1TagEditorPage : UserControl
    {
        string[] PropertyNames = new string[] { "Background", "Foreground", "DisplayMode", "IsTagEditable", "IsEditable", "PlaceHolder", "Separator", "TagWrapping" };

        public C1TagEditorPage()
        {
            InitializeComponent();
            TagEditor.InsertTag("WPF");
        }

        private void C1PropertyGrid_AddingPropertyBox(object sender, C1.WPF.Extended.ChangingPropertyBoxEventArgs e)
        {
            if (!PropertyNames.Contains(e.Property.Name))
                e.Cancel = true;
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            string text = AddInput.Text;
            if (!string.IsNullOrEmpty(text))
            {
                TagEditor.InsertTag(text);
                if (TagEditor.DisplayMode == DisplayMode.Text)
                    TagEditor.UpdateTextFromTags();
                AddInput.Text = "";
            }
        }
    }
}
