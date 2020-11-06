using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.Silverlight;

namespace OutlookBarSamples
{
    public partial class Inbox : UserControl
    {
        public Inbox()
        {
            InitializeComponent();
            mail.ItemsSource = MailFolders.GetData();
        }
    }

    public class MailFolders
    {
        public string Name { get; set; }
        public Image Image { get; set; }
        public string Source { get; set; }
        public List<MailFolders> Children { get; set; }

        public MailFolders()
        {
            this.Name = string.Empty;
            this.Image = null;
            this.Children = new List<MailFolders>();
        }

        public static IEnumerable<MailFolders> GetData()
        {
            List<MailFolders> data = new List<MailFolders>();
            // add root item
            MailFolders inbox = new MailFolders { Name = "Inbox", Source = "../Resources/FolderOpen.png" };

            for (int i = 0; i < 2; i++)
            {
                MailFolders folder = new MailFolders { Name = string.Format("Folder{0}", i), Source = "../Resources/FolderOpen.png" };

                for (int j = 0; j < 8; j++)
                {
                    folder.Children.Add(new MailFolders { Name = string.Format("{0}.{1}", folder.Name, j), Source = "../Resources/FolderOpen.png" });
                }

                inbox.Children.Add(folder);
            }

            data.Add(inbox);

            return data;
        }
    }
}
