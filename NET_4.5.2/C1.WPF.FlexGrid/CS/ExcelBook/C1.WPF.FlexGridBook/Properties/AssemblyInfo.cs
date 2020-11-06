using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ExcelGridBook")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("GrapeCity, Inc.")]
[assembly: AssemblyProduct("ExcelGridBook")]
[assembly: AssemblyCopyright("(c) GrapeCity, Inc..  All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("90a5c843-6d6d-4d3c-b00a-58c4221dc368")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Define XML namespace
[assembly: System.Windows.Markup.XmlnsPrefix("http://schemas.componentone.com/winfx/2006/xaml", "c1")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.componentone.com/winfx/2006/xaml", "C1.WPF.FlexGridBook")]

// Stuff required by WPF:
[assembly: System.Windows.ThemeInfo(
    System.Windows.ResourceDictionaryLocation.None,
    // where theme specific resource dictionaries are located
    // (used if a resource is not found in the page, 
    // or application resource dictionaries)
    System.Windows.ResourceDictionaryLocation.SourceAssembly
    // where the generic resource dictionary is located
    // (used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]
