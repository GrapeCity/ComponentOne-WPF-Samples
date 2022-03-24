using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C1DataCollection101
{
    public class Group : ObservableCollection<Item>
    {
        public Group(string groupName, IEnumerable<Item> items)
            : base(items)
        {
            GroupName = groupName;
        }
        public string GroupName { get; set; }
    }
}
