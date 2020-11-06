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
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF;
using C1.WPF.Core;
using C1.WPF.Docking;
using C1.WPF.Menu;
using C1.WPF.TabControl;

namespace DockingExplorer
{
    /// <summary>
    /// Interaction logic for VisualStudioLookPage.xaml
    /// </summary>
    public partial class VisualStudioLookPage : UserControl
    {
        public VisualStudioLookPage()
        {
            InitializeComponent();
            Tag = "This sample shows a more realistic layout that looks like Microsoft Visual Studio.";

            UpdateWindowMenuItem();
            dockControl.ItemDockModeChanged += new EventHandler<C1.WPF.Docking.ItemDockModeChangedEventArgs>(MainWindow.dockControl_ItemDockModeChanged);

            Unloaded += delegate
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is WPFWindow)
                        w.Close();
                }
            };
            #region file contents
            App_xaml.Content = new TextBox
            {
                BorderThickness = new Thickness(0),
                Text = @"
<Application xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
             xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
             x:Class=""CollapsedTabItem.App""
             >
    <Application.Resources>
        
    </Application.Resources>
</Application>
"
            };
            Silverlight_js.Content = new TextBox
            {
                BorderThickness = new Thickness(0),
                Text = @"
//v2.0.30511.0
if(!window.Silverlight)window.Silverlight={};Silverlight._silverlightCount=0;Silverlight.__onSilverlightInstalledCalled=false;Silverlight.fwlinkRoot=""http://go2.microsoft.com/fwlink/?LinkID="";Silverlight.__installationEventFired=false;Silverlight.onGetSilverlight=null;Silverlight.onSilverlightInstalled=function(){window.location.reload(false)};Silverlight.isInstalled=function(b){if(b==undefined)b=null;var a=false,m=null;try{var i=null,j=false;if(window.ActiveXObject)try{i=new ActiveXObject(""AgControl.AgControl"");if(b===null)a=true;else if(i.IsVersionSupported(b))a=true;i=null}catch(l){j=true}else j=true;if(j){var k=navigator.plugins[""Silverlight Plug-In""];if(k)if(b===null)a=true;else{var h=k.description;if(h===""1.0.30226.2"")h=""2.0.30226.2"";var c=h.split(""."");while(c.length>3)c.pop();while(c.length<4)c.push(0);var e=b.split(""."");while(e.length>4)e.pop();var d,g,f=0;do{d=parseInt(e[f]);g=parseInt(c[f]);f++}while(f<e.length&&d===g);if(d<=g&&!isNaN(d))a=true}}}catch(l){a=false}return a};Silverlight.WaitForInstallCompletion=function(){if(!Silverlight.isBrows... I'm not copying it all
"
            };
            Web_config.Content = new TextBox
            {
                BorderThickness = new Thickness(0),
                Text = @"
<?xml version=""1.0""?>
<configuration>
	<configSections>
		<sectionGroup name=""system.web.extensions"" type=""System.Web.Configuration.SystemWebExtensionsSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"">
			<sectionGroup name=""scripting"" type=""System.Web.Configuration.ScriptingSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"">
				<section name=""scriptResourceHandler"" type=""System.Web.Configuration.ScriptingScriptResourceHandlerSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" allowDefinition=""MachineToApplication""/>
				<sectionGroup name=""webServices"" type=""System.Web.Configuration.ScriptingWebServicesSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"">
					<section name=""jsonSerialization"" type=""System.Web.Configuration.ScriptingJsonSerializationSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" allowDefinition=""Everywhere""/>
					<section name=""profileService"" type=""System.Web.Configuration.ScriptingProfileServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" allowDefinition=""MachineToApplication""/>
					<section name=""authenticationService"" type=""System.Web.Configuration.ScriptingAuthenticationServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" allowDefinition=""MachineToApplication""/>
					<section name=""roleService"" type=""System.Web.Configuration.ScriptingRoleServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" allowDefinition=""MachineToApplication""/>
				</sectionGroup>
			</sectionGroup>
		</sectionGroup>
	</configSections>
	<appSettings/>
	<connectionStrings/>
	<system.web>
"
            };
            MainPage_xaml.Content = new TextBox
            {
                BorderThickness = new Thickness(0),
                Text = @"
<UserControl
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:sys=""clr-namespace:System;assembly=mscorlib""
    xmlns:c1=""http://schemas.componentone.com/winfx/2006/xaml""
    xmlns:local=""clr-namespace:VSDev9""
    xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
    xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
    mc:Ignorable=""d""
    x:Class=""VSDev9.MainPage""
    d:DesignWidth=""640"" d:DesignHeight=""480"">
    "
            };
            MainPage_xaml_cs.Content = new TextBox
            {
                BorderThickness = new Thickness(0),
                Text = @"
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
using System.Windows.Threading;
using C1.Silverlight.Docking;
using System.Diagnostics;
using C1.Silverlight;
using C1.Silverlight.RichTextBox;

namespace VSDev9
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            UpdateWindowMenuItem();

            #region file contents
            App_xaml.Content = new C1RichTextBoxHtml
            {
                BorderThickness = new Thickness(0),
                Text = @""
<Application xmlns=""""http://schemas.microsoft.com/winfx/2006/xaml/presentation""""
             xmlns:x=""""http://schemas.microsoft.com/winfx/2006/xaml""""
             x:Class=""""CollapsedTabItem.App""""
             >
    <Application.Resources>
        
    </Application.Resources>
</Application>
""
"
            };
            C1RichTextBoxHtml_cs.Content = new TextBox
            {
                BorderThickness = new Thickness(0),
                Text = @"
using System.Linq;
using System.Windows.Media;

using C1.Silverlight.RichTextBox;
using C1.Silverlight.RichTextBox.Documents;
using System.Text.RegularExpressions;

namespace VSDev9
{
    public class C1RichTextBoxHtml
        : C1RichTextBox
    {
        C1RangeStyleCollection _rangeStyles = new C1RangeStyleCollection();

        // initialize regular expression used to parse HTML
        string tagPattern =
            @""</?(?<tagName>[\.a-zA-Z0-9_:\-]+)"" +
            @""(\s+(?<attName>[\.a-zA-Z0-9_:\-]+)(?<attValue>(=""[^""]+"")?))*\s*/?>"";

"
            };
            App_xaml_cs.Content = new TextBox
            {
                BorderThickness = new Thickness(0),
                Text = @"
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

namespace VSDev9
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }
"
            };
            Default_html.Content = new TextBox
            {
                BorderThickness = new Thickness(0),
                Text = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" >

<head>
    <title>VSDev9_2008</title>
    <style type=""text/css"">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }
    </style>
    <script type=""text/javascript"" src=""Silverlight.js""></script>
    <script type=""text/javascript"">
        function onSilverlightError(sender, args) {
            ...
        }
    </script>
</head>
<body>
    <form id=""form1"" runat=""server"" style=""height:100%"">
    <div id=""silverlightControlHost"">
        <object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" width=""100%"" height=""100%"">
		  <param name=""source"" value=""ClientBin/VSDev9.xap""/>
		  <param name=""onError"" value=""onSilverlightError"" />
		  <param name=""background"" value=""white"" />
		  <param name=""minRuntimeVersion"" value=""3.0.40624.0"" />
		  <param name=""autoUpgrade"" value=""true"" />
		  <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"" style=""text-decoration:none"">
 			  <img src=""http://go.microsoft.com/fwlink/?LinkId=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none""/>
		  </a>
	    </object><iframe id=""_sl_historyFrame"" style=""visibility:hidden;height:0px;width:0px;border:0px""></iframe></div>
    </form>
</body>
</html>
"
            #endregion

            };
        }


        int item_count;

        void AddDocking_Click(object sender, SourcedEventArgs e)
        {
            Add(DockMode.Docked);
        }
        void AddSliding_Click(object sender, SourcedEventArgs e)
        {
            Add(DockMode.Sliding);
        }

        void Add(DockMode dockMode)
        {
            dockControl.Items.Insert(0, new C1DockTabControl
            {
                DockMode = dockMode,
                Items =
                {
                    new C1DockTabItem
                    {
                        Header = "tab " + item_count,
                        Content = "tab " + item_count,                        
                    },
                },
            });
            item_count++;
        }

        private void ToggleWindowMode_Click(object sender, SourcedEventArgs e)
        {
            var value = (DockMode)(sliding.DockMode + 1);
            if (!Enum.IsDefined(typeof(DockMode), value))
            {
                value = 0;
            }
            sliding.DockMode = value;
        }

        private void ToggleTabStripPlacement_Click(object sender, SourcedEventArgs e)
        {
            sliding.TabStripPlacement = (Dock)(sliding.TabStripPlacement + 1);
            if (!Enum.IsDefined(typeof(Dock), sliding.TabStripPlacement))
            {
                sliding.TabStripPlacement = 0;
            }
        }


        private void dockControl_PickerLoading(object sender, PickerLoadingEventArgs e)
        {
            if (docs.Items.Contains(e.Source))
            {
                e.ShowLeftInnerPart = false;
                e.ShowLeftOuterPart = false;
                e.ShowTopInnerPart = false;
                e.ShowTopOuterPart = false;
                e.ShowRightInnerPart = false;
                e.ShowRightOuterPart = false;
                e.ShowBottomInnerPart = false;
                e.ShowBottomOuterPart = false;
                e.ShowOverInnerPart = false;
            }
        }

        private void docs_TabClosed(object sender, SourcedEventArgs e)
        {
            var tabItem = e.Source as C1TabItem;
            txtOutput.Text += tabItem.Header.ToString() + " closed...\n";
        }

        private void docs_ItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateWindowMenuItem();
        }

        private void UpdateWindowMenuItem()
        {
            if (WindowMenuItem == null | docs == null)
                return;
            WindowMenuItem.Items.Clear();
            foreach (var item in docs.Items)
            {
                var tabItem = (C1DockTabItem)item; // must be fresh each loop iteration because of the delegate
                var menuItem = new C1MenuItem();
                menuItem.Header = tabItem.Header;
                menuItem.Click += delegate
                {
                    tabItem.IsSelected = true;
                };
                WindowMenuItem.Items.Add(menuItem);
            }
        }

        private void dockControl_ViewChanged(object sender, EventArgs e)
        {
            var list = new List<C1MenuItem>();
            foreach (var nestedTabControl in dockControl.NestedItems)
            {
                var tabControl = nestedTabControl; // must be fresh each loop iteration because of the delegate
                if (tabControl == docs)
                    continue;
                foreach (var item in tabControl.Items)
                {
                    var tabItem = (C1DockTabItem)item; // must be fresh each loop iteration because of the delegate
                    var menuItem = new C1MenuItem();
                    menuItem.Header = tabItem.Header;
                    menuItem.Click += delegate
                    {
                        switch (tabControl.DockMode)
                        {
                            case DockMode.Hidden:
                                tabControl.DockMode = DockMode.Docked;
                                break;
                            case DockMode.Sliding:
                                tabControl.SlideOpen();
                                break;
                        }
                        tabItem.IsSelected = true;
                    };
                    list.Add(menuItem);
                }
            }

            ViewMenuItem.Items.Clear();
            foreach (var menuItem in list.OrderBy(mi => mi.Header))
            {
                ViewMenuItem.Items.Add(menuItem);
            }
        }

        private void File_Click(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = (TreeViewItem)sender;
            var iconText = (IconText)treeViewItem.Header;
            var tabItem = (C1TabItem)FindName(iconText.Text.Replace('.', '_'));
            if (tabItem.Parent != docs)
            {
                HiddenTabs.Children.Remove(tabItem);
                docs.Items.Add(tabItem);
            }
            (tabItem.Parent as C1TabControl).SelectedItem = tabItem;
            //tabItem.IsSelected = true;
        }

    }
}
