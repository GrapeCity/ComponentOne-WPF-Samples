using C1.WPF.Extended;
using C1.WPF.Extended.PropertyGrid;

namespace ControlExplorer
{
    public class ReadOnlyImageSourceEditor
        : ImageSourceEditor
    {
        public ReadOnlyImageSourceEditor() 
        {
            IsReadOnly = true;
            IsEnabled = false;
        }

        public override ITypeEditorControl Create()
        {
            return new ReadOnlyImageSourceEditor();
        }
    }
}
