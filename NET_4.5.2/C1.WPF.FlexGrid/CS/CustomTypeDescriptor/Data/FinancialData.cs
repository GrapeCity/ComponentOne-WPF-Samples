using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;

namespace CustomTypeDescriptor
{
    /// <summary>
    /// Based on this: http://en.wikipedia.org/wiki/Market_data
    /// 
    /// This version implements ICustomTypeDescriptor to provide a 
    /// custom list of bindable properties, and can only be used
    /// in WPF (Silverlight does not support the ICustomTypeDescriptor
    /// interface).
    /// </summary>
    public class FinancialData : INotifyPropertyChanged, ICustomTypeDescriptor
    {
        const int HISTORY_SIZE = 5;

        List<decimal> _askHistory = new List<decimal>();
        List<decimal> _bidHistory = new List<decimal>();
        List<decimal> _saleHistory = new List<decimal>();

        static PropertyDescriptorCollection _propertyDescriptorCollectionCache;
        static PropertyDescriptorCollection _zeroPropertyDescriptorCollection = new PropertyDescriptorCollection(null);
        internal Dictionary<string, object> _propBag = new Dictionary<string, object>();

        public List<decimal> GetHistory(string propName)
        {
            switch (propName)
            {
                case "Ask":
                    return _askHistory;
                case "Bid":
                    return _bidHistory;
                case "LastSale":
                    return _saleHistory;
            }
            return null;
        }
        internal void AddHistory(List<decimal> list, decimal value)
        {
            while (list.Count >= HISTORY_SIZE)
            {
                list.RemoveAt(0);
            }
            while (list.Count < HISTORY_SIZE)
            {
                list.Add(value);
            }
        }

        internal object GetProp(string propName)
        {
            object value = null;
            _propBag.TryGetValue(propName, out value);
            return value;
        }
        internal void SetProp(string propName, object value)
        {
            if (!_propBag.ContainsKey(propName) || GetProp(propName) != value)
            {
                _propBag[propName] = value;

                // save history (for sparklines)
                switch (propName)
                {
                    case "Bid":
                    case "Ask":
                    case "LastSale":
                        AddHistory(GetHistory(propName), (decimal)Convert.ChangeType(value, typeof(decimal)));
                        break;
                }

                OnPropertyChanged(new PropertyChangedEventArgs(propName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        // get a default list of financial items
        public static FinancialDataList GetFinancialData()
        {
            var list = new FinancialDataList();
            var rnd = new Random(0);
            var asm = Assembly.GetExecutingAssembly();
            foreach (string resName in asm.GetManifestResourceNames())
            {
                if (resName.EndsWith("data.zip"))
                {
                    var zip = new C1.C1Zip.C1ZipFile(asm.GetManifestResourceStream(resName));
                    using (var sr = new StreamReader(zip.Entries["StockSymbols.txt"].OpenReader()))
                    {
                        while (!sr.EndOfStream)
                        {
                            var sn = sr.ReadLine().Split('\t');
                            if (sn.Length > 1 && sn[0].Trim().Length > 0)
                            {
                                var data = new FinancialData();
                                data.SetProp("Symbol", sn[0]);
                                data.SetProp("Name", sn[1]);
                                data.SetProp("Bid", rnd.Next(1, 1000));
                                data.SetProp("Ask", (decimal)rnd.NextDouble());
                                data.SetProp("LastSale", (decimal)rnd.NextDouble());
                                data.SetProp("BidSize", rnd.Next(10, 500));
                                data.SetProp("AskSize", rnd.Next(10, 500));
                                data.SetProp("LastSize", rnd.Next(10, 500));
                                data.SetProp("Volume", rnd.Next(10000, 20000));
                                data.SetProp("QuoteTime", DateTime.Now);
                                data.SetProp("TradeTime", DateTime.Now);
                                list.Add(data);
                                FinancialData.BuildPropertyDescriptorCollection(data._propBag, false);
                            }
                        }
                    }
                    list.AutoUpdate = true;
                    return list;
                }
            }
            throw new Exception("Can't find 'data.zip' embedded resource.");
        }

        /// <summary>
        /// Can now call this multiple times and append to existing collection, if desired,
        ///   or do minimal work if propertydescriptor set already exists.
        /// </summary>
        public static void BuildPropertyDescriptorCollection(Dictionary<string, object> properties, bool append)
        {
            // return if no source information from which to build propertydescriptor set...
            if ((null == properties) || (0 == properties.Count))
            {
                return;
            }

            // return if set already exists and we are not to append...
            if (!append && (null != _propertyDescriptorCollectionCache))
            {
                return;
            }

            var descriptors = new List<PropertyDescriptor>();

            // copy over any existing PropertyDescriptors (append)
            if (null != _propertyDescriptorCollectionCache)
            {
                foreach (PropertyDescriptor pd in _propertyDescriptorCollectionCache)
                    descriptors.Add(pd);
            }

            // add each supplied descriptor, so long as  
            //    no descriptor with same name already exists
            foreach (KeyValuePair<string, object> kvp in properties)
            {
                if (!descriptors.Exists(p => p.Name == kvp.Key))
                    descriptors.Add(new ItemPropertyDescriptor(kvp.Key, kvp.Value));
            }

            // copy over to the READONLY collection used to supply descriptor for given property name
            _propertyDescriptorCollectionCache = new PropertyDescriptorCollection(descriptors.ToArray());
        }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return new AttributeCollection(null);
        }

        public string GetClassName()
        {
            return null;
        }

        public string GetComponentName()
        {
            return null;
        }

        public TypeConverter GetConverter()
        {
            return null;
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return null;
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        public EventDescriptorCollection GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            if (_propertyDescriptorCollectionCache == null)
            {
                return _zeroPropertyDescriptorCollection;
            }
            return _propertyDescriptorCollectionCache;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }
    public class FinancialDataList : List<FinancialData>
    {
        // fields
        System.Windows.Threading.DispatcherTimer _timer;
        Random _rnd = new Random(0);

        // object model
        public int UpdateInterval 
        { 
            get { return (int)_timer.Interval.TotalMilliseconds; }
            set { _timer.Interval = TimeSpan.FromMilliseconds(value); }
        }
        public int BatchSize { get; set; }
        public bool AutoUpdate
        {
            get { return _timer.IsEnabled; }
            set { _timer.IsEnabled = value; }
        }

        // ctor
        public FinancialDataList()
        {
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Tick += _timer_Tick;
            UpdateInterval = 100;
            BatchSize = 100;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (this.Count > 0)
            {
                for (int i = 0; i < BatchSize; i++)
                {
                    int index = _rnd.Next() % this.Count;
                    var data = this[index];
                    decimal bid = 10 * (decimal)(1 + (_rnd.NextDouble() * .11 - .05));
                    decimal ask = 20 * (decimal)(1 + (_rnd.NextDouble() * .11 - .05));
                    data.SetProp("Bid", bid);
                    data.SetProp("Ask", ask);
                    data.SetProp("BidSize", _rnd.Next(10, 1000));
                    data.SetProp("AskSize", _rnd.Next(10, 1000));
                    var sale = (ask + bid) / 2;
                    data.SetProp("LastSale", sale);
                    data.SetProp("LastSize", (_rnd.Next(10, 1000) + _rnd.Next(10, 1000)) / 2);
                    data.SetProp("QuoteTime", DateTime.Now);
                    data.SetProp("TradeTime", DateTime.Now.AddSeconds(-_rnd.Next(0, 60)));
                }
            }
        }
    }

    public class ItemPropertyDescriptor : PropertyDescriptor
    {
        #region private instance data

        private Type mPropType = typeof(object);

        #endregion

        public string Key
        { get; set; }


        public ItemPropertyDescriptor(string key)
            : base(key, null)
        {
            Key = key;
        }

        public ItemPropertyDescriptor(string key, object sampleValue)
            : this(key)
        {
            if (null != sampleValue)
                mPropType = sampleValue.GetType();
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override System.Type ComponentType
        {
            get { return typeof(FinancialData); }
        }

        public override object GetValue(object component)
        {
            object retVal = null;
            FinancialData trade = (FinancialData)component;
            if (null != trade)
                retVal = trade.GetProp(Key);
            return retVal;
        }

        public override void ResetValue(object component)
        {
           ;
        }

        public override void SetValue(object component, object value)
        {
            FinancialData trade = (FinancialData)component;
            if (null != trade)
            {
                trade.SetProp(Key, value);
            }
            OnValueChanged(component, EventArgs.Empty);
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return mPropType; }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
