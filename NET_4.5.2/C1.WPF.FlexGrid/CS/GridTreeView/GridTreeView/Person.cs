using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;

namespace GridTreeView
{
    /// <summary>
    /// Data class: Each Person object has a Children property that is a list of
    /// Person objects.
    /// </summary>
    public class Person : INotifyPropertyChanged
    {
        static int _ctr;
        ObservableCollection<Person> _children;
        int _id;
        string _name;
        Person _parent;

        public Person()
        {
            ID = ++_ctr;
            Name = string.Format("Person {0}", ID);
            _children = new ObservableCollection<Person>();
            _children.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (Person child in e.NewItems)
                    {
                        child.Parent = this;
                    };
                }
            };
        }

        [Display(Name = "ID")]
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        [Display(Name = "Name")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [Display(Name = "Parent")]
        public Person Parent
        {
            get { return _parent; }
            protected set { _parent = value; }
        }
        public ObservableCollection<Person> Children
        {
            get { return _children; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
