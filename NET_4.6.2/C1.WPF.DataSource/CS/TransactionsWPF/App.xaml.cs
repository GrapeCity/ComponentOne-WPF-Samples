using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using System.Reflection;

namespace ClientTransactions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Specify where Entity Framework should look for the Northwind database.
            string folder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"..\..\..\..\Data"));
            string dbFile = Path.Combine(folder, "Northwnd.mdf");
            AppDomain.CurrentDomain.SetData("DataDirectory", folder);
            if (!File.Exists(dbFile))
                throw new Exception("Sample database Northwnd.mdf must be created in the Data folder. Run the CreateSampleDB utility to create the Northwind database");

            // make sure both version 11 and version 12 or higher of SQLServer LocalDb are supported
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions\12.0");
            string ver = key == null ? "v11.0" : "MSSQLLocalDB";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connStr = config.ConnectionStrings.ConnectionStrings["NORTHWNDEntities"];
            connStr.ConnectionString = connStr.ConnectionString.Replace("v11.0", ver);
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");

            base.OnStartup(e);
        }
    }
}
