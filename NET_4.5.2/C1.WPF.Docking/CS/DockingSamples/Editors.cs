using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.Extended;
using System.ComponentModel;
using C1.Silverlight;
using System.Windows.Data;

namespace DockingSamples
{
    public abstract class ComboEditor : ComboBox
    {
        public bool Supports(PropertyAttribute Property)
        {
            throw new NotImplementedException();
        }

        public void Attach(PropertyAttribute property)
        {
            // Bind Property to SelectedItem
            Binding editorBinding = new Binding(property.MemberName);
            editorBinding.Mode = BindingMode.TwoWay;
            editorBinding.Source = property.SelectedObject;
            this.SetBinding(SelectedItemProperty, editorBinding);
        }

        public void Detach(PropertyAttribute property)
        {
            ClearValue(SelectedItemProperty);
        }

        public event PropertyChangedEventHandler ValueChanged;
    }
    public class FontFamilyEditor : ComboEditor, ITypeEditorControl
    {
        public FontFamilyEditor()
        {
            Items.Add(new FontFamily("Arial"));
            Items.Add(new FontFamily("Arial Black"));
            Items.Add(new FontFamily("Arial Unicode MS"));
            Items.Add(new FontFamily("Calibri"));
            Items.Add(new FontFamily("Cambria"));
            Items.Add(new FontFamily("Cambria Math"));
            Items.Add(new FontFamily("Comic Sans MS"));
            Items.Add(new FontFamily("Candara"));
            Items.Add(new FontFamily("Consolas"));
            Items.Add(new FontFamily("Constantia"));
            Items.Add(new FontFamily("Corbel"));
            Items.Add(new FontFamily("Courier New"));
            Items.Add(new FontFamily("Georgia"));
            Items.Add(new FontFamily("Lucida Grande"));
            Items.Add(new FontFamily("Segoe UI"));
            Items.Add(new FontFamily("Symbol"));
            Items.Add(new FontFamily("Tahoma"));
            Items.Add(new FontFamily("Times New Roman"));
            Items.Add(new FontFamily("Trebuchet MS"));
            Items.Add(new FontFamily("Verdana"));
            Items.Add(new FontFamily("Wingdings"));
            Items.Add(new FontFamily("Wingdings 2"));
            Items.Add(new FontFamily("Wingdings 3"));
        }
        public ITypeEditorControl Create()
        {
            return new FontFamilyEditor();
        }
    }
    public class FontStretchEditor : ComboEditor, ITypeEditorControl
    {
        public FontStretchEditor()
        {
            Items.Add(FontStretches.Condensed);
            Items.Add(FontStretches.Expanded);
            Items.Add(FontStretches.ExtraCondensed);
            Items.Add(FontStretches.ExtraExpanded);
            Items.Add(FontStretches.Normal);
            Items.Add(FontStretches.SemiCondensed);
            Items.Add(FontStretches.SemiExpanded);
            Items.Add(FontStretches.UltraCondensed);
            Items.Add(FontStretches.UltraExpanded);
        }
        public ITypeEditorControl Create()
        {
            return new FontStretchEditor();
        }
    }
    public class FontStyleEditor : ComboEditor, ITypeEditorControl
    {
        public FontStyleEditor()
        {
            Items.Add(FontStyles.Normal);
            Items.Add(FontStyles.Italic);
        }
        public ITypeEditorControl Create()
        {
            return new FontStyleEditor();
        }
    }
    public class FontWeightEditor : ComboEditor, ITypeEditorControl
    {
        public FontWeightEditor()
        {
            Items.Add(FontWeights.Thin);
            Items.Add(FontWeights.ExtraLight);
            Items.Add(FontWeights.Light);
            Items.Add(FontWeights.Normal);
            Items.Add(FontWeights.Medium);
            Items.Add(FontWeights.SemiBold);
            Items.Add(FontWeights.Bold);
            Items.Add(FontWeights.ExtraBold);
            Items.Add(FontWeights.Black);
            Items.Add(FontWeights.ExtraBlack);
        }
        public ITypeEditorControl Create()
        {
            return new FontWeightEditor();
        }
    }
}
