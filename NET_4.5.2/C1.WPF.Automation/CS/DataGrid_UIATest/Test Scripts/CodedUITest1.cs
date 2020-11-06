using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
using Microsoft.VisualStudio.TestTools.UITest.Common.UIMap;
using System.Linq;


namespace TestProject1
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
        public CodedUITest1()
        {
        }

        const string c1datagrid = "c1datagrid";

        public WpfWindow Get_Window()
        {
            var window = new WpfWindow();
            window.SearchProperties[WpfWindow.PropertyNames.Name] = "C1DataGrid Test Automation";
            return window;
        }

        public WpfTable Get_Grid()
        {          
            var grid = new WpfTable(Get_Window());
            grid.SearchProperties[WpfTable.PropertyNames.AutomationId] = c1datagrid;

            return grid;
        }

        public WpfButton Get_Button(string AutomationId)
        {
            var btn = new WpfButton(Get_Window());
            btn.SearchProperties[WpfButton.PropertyNames.AutomationId] = AutomationId;
            return btn;
        }
     
        public List<WpfCustom> Get_C1DateTimePickers_In_Grid()
        {           
            var dtp = new WpfCustom(Get_Grid());
            dtp.SearchProperties[WpfCustom.PropertyNames.ClassName] = "Uia.C1DateTimePicker";
            return dtp.FindMatchingControls().Cast<WpfCustom>().ToList();
        }

        [TestMethod]
        public void Edit_DateColumn_With_C1DateTimePicker()
        {
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            var msg = "Default:" + Get_C1DateTimePickers_In_Grid().Count;
            Mouse.Click(Get_Button("btn_BeginEdit"));
            msg += ".AfterBeginEdit:" + Get_C1DateTimePickers_In_Grid().Count;            
            Assert.AreEqual("Default:0.AfterBeginEdit:1", msg);
        }

        [TestMethod]
        public void GrouBy()
        {
            var msg = "Default:" + Get_Grid().RowCount;
            Mouse.Click(Get_Button("btn_GroupBy"));
            msg += ".AfterBeginEdit:" + Get_Grid().RowCount;            
            Assert.AreEqual("Default:12.AfterBeginEdit:14", msg);
        }

        [TestMethod]
        public void Filter()
        {         
            var msg = "Default:" + Get_Grid().RowCount;
            var body = new WpfPane(Get_Grid());
            body.SearchProperties[WpfPane.PropertyNames.AutomationId] = "Body";
            var cell = new WpfCustom(body);
            cell.SearchProperties[WpfCustom.PropertyNames.ClassName] = "Uia.DataGridCellPresenter";
            var filterbox = new WpfEdit(cell);
            filterbox.Text = "Closed";
            msg += ".AfterFilter:" + Get_Grid().RowCount;            
            Assert.AreEqual("Default:12.AfterFilter:7", msg);
        }

        [TestCleanup]
        public void Reset_Data()
        {
            Mouse.Click(Get_Button("btn_Reset"));
        }
       

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //    // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //    // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;

    }
}
