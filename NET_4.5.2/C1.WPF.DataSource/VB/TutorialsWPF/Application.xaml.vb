Imports System.IO
Imports System.Reflection
Imports System.Configuration
Imports C1.Data.Entities

Class Application

    Public Shared ClientCache As EntityClientCache

    Private Sub Application_Startup(sender As Object, e As System.Windows.StartupEventArgs) Handles Me.Startup
        ' Specify where Entity Framework should look for the Northwind database.
        Dim folder As String = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "..\..\..\..\Data"))
        Dim dbFile As String = Path.Combine(folder, "Northwnd.mdf")
        AppDomain.CurrentDomain.SetData("DataDirectory", folder)
        If Not File.Exists(dbFile) Then
            Throw New Exception("Sample database Northwnd.mdf must be created in the Samples\\Data folder. Run the CreateSampleDB utility to create the Northwind database")
        End If

        ' make sure both version 11 and version 12 or higher of SQLServer LocalDb are supported
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions\12.0")
        Dim ver As String = If(Key Is Nothing, "v11.0", "MSSQLLocalDB")
        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim connStr = config.ConnectionStrings.ConnectionStrings("NORTHWNDEntities")
        connStr.ConnectionString = connStr.ConnectionString.Replace("v11.0", ver)
        config.Save(ConfigurationSaveMode.Modified, True)
        ConfigurationManager.RefreshSection("connectionStrings")

        ' Get a global ClientCache and save it in a static field
        ' so it is accessible from any class of the project.
        ClientCache = EntityClientCache.GetDefault(GetType(NORTHWNDEntities))

    End Sub
End Class
