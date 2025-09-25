using System.Windows;

namespace MapsExplorer.Sources
{
    /// <summary>
    /// Interaction logic for RequestKeyDialog.xaml
    /// </summary>
    public partial class RequestKeyDialog : Window
    {
        /// <summary>
        /// Creates a new instance of the <see cref="RequestKeyDialog"/> class.
        /// </summary>
        public RequestKeyDialog() : this(null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RequestKeyDialog"/> class.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        public RequestKeyDialog(string title)
        {
            InitializeComponent();
            DataContext = this;

            if (title != null)
            {
                Title = title;
            }
        }

        /// <summary>
        /// Gets or sets API key.
        /// </summary>
        public string ApiKey { get; set; }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}