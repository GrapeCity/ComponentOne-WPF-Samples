using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexGridExplorer.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class CustomTypeCustomer : ICustomTypeDescriptor
    {
        private readonly Dictionary<string, object> _data;

        public CustomTypeCustomer()
        {
            _data = new Dictionary<string, object>();
        }
        
        public CustomTypeCustomer(Dictionary<string, object> initialData)
        {
            _data = new Dictionary<string, object>(initialData ?? new Dictionary<string, object>());
        }
        public object this[string name]
        {
            get => _data.TryGetValue(name, out var val) ? val : null;
            set => _data[name] = value;
        }
        public void AddProperty(string name, object value = null)
        {
            _data[name] = value;
        }
        public PropertyDescriptorCollection GetProperties()
        {
            var descriptors = new List<PropertyDescriptor>();
            foreach (var key in _data.Keys)
            {
                descriptors.Add(new CustomPropertyDescriptor(key));
            }
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => GetProperties();
        public AttributeCollection GetAttributes() => AttributeCollection.Empty;
        public string GetClassName() => null;
        public string GetComponentName() => null;
        public TypeConverter GetConverter() => null;
        public EventDescriptor GetDefaultEvent() => null;
        public PropertyDescriptor GetDefaultProperty() => null;
        public object GetEditor(Type editorBaseType) => null;
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => EventDescriptorCollection.Empty;
        public EventDescriptorCollection GetEvents() => EventDescriptorCollection.Empty;
        public object GetPropertyOwner(PropertyDescriptor pd) => this;

        // Inner PropertyDescriptor class
        private class CustomPropertyDescriptor : PropertyDescriptor
        {
            public CustomPropertyDescriptor(string name) : base(name, null) { }

            public override bool CanResetValue(object component) => false;
            public override Type ComponentType => typeof(CustomTypeCustomer);
            public override object GetValue(object component) => ((CustomTypeCustomer)component)[Name];
            public override bool IsReadOnly => false;
            public override Type PropertyType => typeof(object);
            public override void ResetValue(object component) { }
            public override void SetValue(object component, object value) => ((CustomTypeCustomer)component)[Name] = value;
            public override bool ShouldSerializeValue(object component) => true;
        }
    }

}
