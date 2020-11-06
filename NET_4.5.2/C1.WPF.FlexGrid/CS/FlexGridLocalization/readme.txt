FlexGridLocalization
------------------------------------------------------------------------------------
Shows how you can localize the FlexGrid filter to different languages.

The ComponentOne Studio for WPF product ships with satellite dlls that provide
control localization to several languages. In addition to English, the ComponentOne 
controls support Arabic, Danish, German, Spanish, Finnish, French, Hebrew, Italian, 
Japanese, Dutch, Norwegian, Portuguese, Russian, and Swedish.

This sample shows how you can configure your WPF application to use the
strings in any of these languages or to define your own in case the language you 
want to use is not on the above list or in case you want to customize the strings.

To localize your WPF application, follow these steps:

1) Choose the language(s) you want to support and find the corresponding two-letter
CultureInfo code. For example German is "de", Spanish is "es", and French is "fr".


2) If your list includes any languages not on the above list, then you have to create
those satellite dlls yourself. In our example, this Hindi language is not supported, so we have to add those resources. To do this, follow these steps:

- Add a new folder called "Resources" to your application.

- Add a new item to the of type "Resources File" to the "Resources" folder.

- Name the new file "C1.WPF.FlexGridFilter.hi.resx". Notice that the name of the file
specifies the item that will be localized and also the language that it will contain.

- Double-click the new file and enter the names and values for all the resources. This sample
contains the names of the resources used by the FlexGridFilter.This file contains some dummy text


3) At this point, the application contains all the resources required, and the only item left
is for you to specify the language that you want to use. This can be done in the Application_Startup
method (in the App.xaml.cs file) or on the window itself. This sample specifies the culture to use
in the MainWindow.xaml.cs file:

<code>
     public MainWindow()
        {
            // select the culture (de, it, fr, tr are supported)
            var ci = new System.Globalization.CultureInfo("hi"); // de-DE, etc

            // this controls the UI strings
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;


            // initialize the component
            InitializeComponent();
		}
</code>


