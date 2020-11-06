using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StockAnalysis.Data
{
    public class PropertyParam
    { 
        public PropertyParam(string key, Type type, object group = null)
        {
            this.Key = key;
            this.Type = type;
            this.Group = group;
        }

        public string Key
        {
            get;
            set;
        }
        public Type Type
        {
            get;
            set;
        }
        public object Group
        {
            get;
            set;
        }


    }
}
