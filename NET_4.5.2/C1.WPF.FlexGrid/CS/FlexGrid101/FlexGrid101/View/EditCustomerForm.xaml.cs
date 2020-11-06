using FlexGrid101.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for EditCustomerForm.xaml
    /// </summary>
    public partial class EditCustomerForm : Window
    {
        private Customer editingCustomer;
        public EditCustomerForm(Customer cust)
        {
            InitializeComponent();

            if (cust != null)
            {
                // initialize input form with values from the selected customer
                this.editingCustomer = cust;
                entryFirstName.Text = cust.FirstName;
                entryLastName.Text = cust.LastName;
                datePickerLastOrder.SelectedDate = cust.LastOrderDate;
                entryOrderTotal.Text = cust.OrderTotal.ToString("n4");
            }

            Title = AppResources.EditCustomer;
            btnApply.Content = AppResources.Apply;
            btnApply.Click += BtnApply_Click;
            btnCancel.Content = AppResources.Cancel;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            // save new values to the customer object
            this.editingCustomer.FirstName = entryFirstName.Text;
            this.editingCustomer.LastName = entryLastName.Text;
            this.editingCustomer.LastOrderDate = datePickerLastOrder.SelectedDate.Value;
            double orderTotal;
            if (double.TryParse(entryOrderTotal.Text, out orderTotal))
                this.editingCustomer.OrderTotal = orderTotal;

            Close();
        }
    }
}
