using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StockAnalysis.Data
{
    public class SettingParam : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string proName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(proName));
            }
        }


        public SettingParam(string key, Type type, object value, string description = null, IEnumerable<object> enumSource = null)
        {
            this.Key = key;
            this.Description = description;
            this.Type = type;
            this.Value = value;
            this.EnumSource = enumSource;
        }

        public SettingParam(string key, Type type, object value, object group, string description = null, IEnumerable<object> enumSource = null)
        {
            this.Key = key;
            this.Description = description;
            this.Type = type;
            this.Value = value;
            this.EnumSource = enumSource;
            this.Group = group;
        }

        public SettingParam(string key, Type type, object value, double minimum, object group = null, string description = null, IEnumerable<object> enumSource = null)
        {
            this.Key = key;
            this.Description = description;
            this.Type = type;
            this.Value = value;
            this.Minimum = minimum;
            this.EnumSource = enumSource;
            this.Group = group;
        }

        public string Key
        {
            get;
            set;
        }

        private string description;
        public string Description
        {
            get
            {
                return string.IsNullOrEmpty(description) ? this.Key : description;
            }
            set
            {
                if (this.description != value)
                {
                    description = value;
                }
            }
        }

        private Type type = null;
        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                if (this.type != value)
                {
                    this.type = value;
                    this.OnPropertyChanged("Type");
                }
            }
        }

        public bool IsEnumType
        {
            get
            {
                return Type.IsEnum;
            }
        }

        public IEnumerable<object> EnumSource
        {
            get;
            set;
        }


        private object value;
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    this.OnPropertyChanged("Value");
                }
            }
        }

        private double minimum = 1;
        public double Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                if (this.minimum != value)
                {
                    this.minimum = value;
                    this.OnPropertyChanged("Minimum");
                }
            }
        }

        private bool isEditable = true;
        public bool IsEditable
        {
            get
            {
                return this.isEditable;
            }
            set
            {
                this.isEditable = value;
                this.OnPropertyChanged("IsEditable");
            }
        }

        private object group = null;
        public object Group
        {
            get
            {
                return group;
            }
            set
            {
                if (this.group != value)
                {
                    this.group = value;
                    this.OnPropertyChanged("Group");
                }
            }
        }


    }
}
