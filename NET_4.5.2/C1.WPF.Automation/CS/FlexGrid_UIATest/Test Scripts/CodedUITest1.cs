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

namespace Test_Scripts
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

        [TestMethod]
        public void AutoGenerateColumns()
        {            
            var window = new WpfWindow();
            window.SearchProperties[WpfWindow.PropertyNames.Name] = "C1FlexGrid Automation Test";
            var chk_AutoGenerateColumns=new WpfCheckBox(window);
            chk_AutoGenerateColumns.SearchProperties[WpfCheckBox.PropertyNames.AutomationId] = "chk_AutoGenerateColumns";

            var flexgrid = new WpfTable(window);
            flexgrid.SearchProperties[WpfTable.PropertyNames.AutomationId] = "flexgrid";
            var msg = "Default:" + flexgrid.ColumnCount;
            chk_AutoGenerateColumns.Checked = false;
            msg += ".False:" + flexgrid.ColumnCount;
            chk_AutoGenerateColumns.Checked = true;
            Assert.AreEqual("Default:7.False:3", msg);
        }

        [TestMethod]
        public void AddRemoveRow()
        {
            var window = new WpfWindow();
            window.SearchProperties[WpfWindow.PropertyNames.Name] = "C1FlexGrid Automation Test";
            var btn_AddRow = new WpfButton(window);
            btn_AddRow.SearchProperties[WpfButton.PropertyNames.AutomationId] = "btn_AddRow";
            var btn_RemoveRow = new WpfButton(window);
            btn_RemoveRow.SearchProperties[WpfButton.PropertyNames.AutomationId] = "btn_RemoveRow";

            var flexgrid = new WpfTable(window);
            flexgrid.SearchProperties[WpfTable.PropertyNames.AutomationId] = "flexgrid";
            var msg = "Default:" + flexgrid.RowCount;
            Mouse.Click(btn_AddRow);
            Mouse.Click(btn_AddRow);
            msg += ".AfterAddingTwoRows:" + flexgrid.RowCount;
            Mouse.Click(btn_RemoveRow);
            Mouse.Click(btn_RemoveRow);
            msg += ".AfterRemovingTwoRows:" + flexgrid.RowCount;
            Assert.AreEqual("Default:10.AfterAddingTwoRows:12.AfterRemovingTwoRows:10", msg);
        }

        [TestMethod]
        public void SelectedIndex_Copy()
        {
            var window = new WpfWindow();
            window.SearchProperties[WpfWindow.PropertyNames.Name] = "C1FlexGrid Automation Test";
            var btn_SelectedIndex = new WpfButton(window);
            btn_SelectedIndex.SearchProperties[WpfButton.PropertyNames.AutomationId] = "btn_SelectedIndex";
            var btn_Copy = new WpfButton(window);
            btn_Copy.SearchProperties[WpfButton.PropertyNames.AutomationId] = "btn_Copy";

            var flexgrid = new WpfTable(window);
            flexgrid.SearchProperties[WpfTable.PropertyNames.AutomationId] = "flexgrid";
                       
            Mouse.Click(btn_SelectedIndex);
            Mouse.Click(btn_Copy);            
            
            Assert.AreEqual("10", btn_Copy.DisplayText.Replace(Environment.NewLine,string.Empty));
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
