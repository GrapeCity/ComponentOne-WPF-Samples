using FlexGridExplorer.Resources;
using System.Windows;

namespace FlexGridExplorer
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
