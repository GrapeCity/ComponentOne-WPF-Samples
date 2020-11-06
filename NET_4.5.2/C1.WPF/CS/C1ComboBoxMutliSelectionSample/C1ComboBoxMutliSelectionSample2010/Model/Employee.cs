using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C1ComboBoxMutliSelectionSample2010.Model
{
    public enum Gender
    {
        Unknow = 0,
        Male = 1,
        Famle = 2,
    }

    public class Employee: INotifyPropertyChanged
    {
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Name { get; set; }
        public Gender Gender { get; set; } = Gender.Unknow;
        public int Age { get; set; }
        public string Cellphone { get; set; }
        public string Address { get; set; }

        private bool isActived = false;
        public bool IsActived {
            get
            {
                return isActived;
            }
            set
            {
                isActived = value;
                // Call NotifyPropertyChanged when the source property 
                // is updated.
                NotifyPropertyChanged("IsActived");
            }
        }


        private static Employee[] _default;
        public static Employee[] Default
        {
            get
            {
                if (_default == null)
                {
                    _default = GenerateData();
                }
                return _default;
            }
        }

        private static Random _r = new Random();

        public event PropertyChangedEventHandler PropertyChanged;

        private static Employee[] GenerateData()
        {
            Queue<Employee> dq = new Queue<Employee>();

            for (int i = 0; i < 20; i++)
            {
                dq.Enqueue(
                    new Employee()
                    {
                        Name = "User_" + (i + 1),
                        Gender = (Gender)_r.Next(3),
                        Age = _r.Next(100),
                        Cellphone = "13800138000",
                        Address = "Address",
                        IsActived = System.Convert.ToBoolean(_r.Next(2)),
                    }
                );
            }
            
            return dq.ToArray();
        }

    }
}
