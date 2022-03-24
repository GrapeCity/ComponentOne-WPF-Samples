using System;
using System.Windows.Data;
using C1.WPF.DateTimeEditors;
using C1.WPF.Extended;

namespace ControlExplorer
{
    public class TimeSpanEditor : C1TimeEditor, ITypeEditorControl
    {
        #region ITypeEditorControl Members

        public ITypeEditorControl Create()
        {
            return new TimeSpanEditor();
        }

        public TimeSpanEditor()
        {
            this.Format = C1TimeEditorFormat.TimeSpan;
			this.CycleChangesOnBoundaries = false;
        }

        public bool Supports(PropertyAttribute Property)
        {
            return Property.PropertyInfo.PropertyType == typeof(TimeSpan);
        }

        public void Attach(PropertyAttribute property)
        {
            var binding = new Binding(property.PropertyInfo.Name)
            {
                Mode = BindingMode.TwoWay,
                Source = property.SelectedObject,
                ValidatesOnExceptions = true
            };
            this.SetBinding(C1TimeEditor.ValueProperty, binding);
        }

        public void Detach(PropertyAttribute property)
        {
        }

        public event System.ComponentModel.PropertyChangedEventHandler ValueChanged;

        #endregion
    }

 }
