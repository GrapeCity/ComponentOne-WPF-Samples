using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExpressionEditorExplorer
{
    /// <summary>
    /// Sample data class
    /// </summary>
    public class Customer : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private int _age;
        private decimal _orderTotal;
        private int _orderCount;
        private DateTime _lastOrderDate;
        private decimal _spendingLimit;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        public int Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(nameof(Age)); }
        }

        public decimal OrderTotal
        {
            get => _orderTotal;
            set { _orderTotal = value; OnPropertyChanged(nameof(OrderTotal)); }
        }

        public int OrderCount
        {
            get => _orderCount;
            set { _orderCount = value; OnPropertyChanged(nameof(OrderCount)); }
        }

        public DateTime LastOrderDate
        {
            get => _lastOrderDate;
            set { _lastOrderDate = value; OnPropertyChanged(nameof(LastOrderDate)); }
        }

        public decimal SpendingLimit
        {
            get => _spendingLimit;
            set { _spendingLimit = value; OnPropertyChanged(nameof(SpendingLimit)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static ObservableCollection<Customer> GenerateSampleData(int count = 20)
        {
            var firstNames = new[] { "John", "Jane", "Alice", "Bob", "Carol", "David", "Eve", "Frank", "Grace", "Henry" };
            var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Martinez", "Wilson" };
            var rand = new Random();
            var customers = new ObservableCollection<Customer>();

            for (int i = 0; i < count; i++)
            {
                var firstName = firstNames[rand.Next(firstNames.Length)];
                var lastName = lastNames[rand.Next(lastNames.Length)];
                var age = rand.Next(18, 80);
                var orderCount = rand.Next(1, 15);
                var orderTotal = Math.Round((decimal)(rand.NextDouble() * 10000), 2);
                var lastOrderDate = DateTime.Now.AddDays(-rand.Next(0, 365)).AddMinutes(rand.Next(0, 1440));
                var spendingLimit = Math.Round((decimal)(rand.NextDouble() * 20000 + 1000), 2);

                customers.Add(new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    OrderCount = orderCount,
                    OrderTotal = orderTotal,
                    LastOrderDate = lastOrderDate,
                    SpendingLimit = spendingLimit
                });
            }

            return customers;
        }
    }
}
