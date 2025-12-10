using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace C1ComboBoxMutliSelectionSample.Model
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
                    _default = GenerateData(20);
                }
                return _default;
            }
        }

        private static Random _random = new Random();

        public event PropertyChangedEventHandler PropertyChanged;

        public static Employee[] GenerateData(int count)
        {
            Queue<Employee> dq = new Queue<Employee>();

            for (int i = 0; i < count; i++)
            {
                dq.Enqueue(
                    new Employee()
                    {
                        Name = $"{Names[_random.Next(Names.Length)]} {Surnames[_random.Next(Surnames.Length)]}",
                        Gender = (Gender)_random.Next(3),
                        Age = _random.Next(100),
                        Cellphone = "13800138000",
                        Address = "Address",
                        IsActived = Convert.ToBoolean(_random.Next(2)),
                    }
                );
            }
            
            return dq.ToArray();
        }

        private static string[] Names = new string[]
        {
            "Michael",
            "John",
            "Bernardo",
            "Alvaro",
            "Max",
            "Leonard",
            "Noela",
            "Gabriel",
            "Bruno",
            "Rodrigo",
            "Sheela",
            "Chris",
            "Martin",
            "Ben",
            "Alex",
            "Irina",
            "Dave",
            "Patric",
        };
        private static string[] Surnames = new string[]
        {
            "Johnson",
            "Alvarez",
            "Maxtor",
            "Johansen",
            "Vera",
            "García",
            "Days",
            "Castle",
            "Varela",
            "Smith",
            "Gates",
            "Meredith",
            "Drexler",
            "Beckham",
            "Jobs",
        };
    }
}
