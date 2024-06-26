﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 10.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace Test_Scripts
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public partial class UIMap
    {
        
        /// <summary>
        /// AssertMethod1 - Use 'AssertMethod1ExpectedValues' to pass parameters into this method.
        /// </summary>
        public void AssertMethod1()
        {
            #region Variable Declarations
            WpfCheckBox uIItemCheckBox = this.UITestAutomationWindow.UIFlexgridTable.UIRow0DataItem.UIItem01DataItem.UIItemCheckBox;
            #endregion

            // Verify that 'Unknown Name' check box's property 'ControlType' equals 'CheckBox'
            Assert.AreEqual(this.AssertMethod1ExpectedValues.UIItemCheckBoxControlType, uIItemCheckBox.ControlType.ToString());
        }
        
        /// <summary>
        /// AssertMethod2 - Use 'AssertMethod2ExpectedValues' to pass parameters into this method.
        /// </summary>
        public void AssertMethod2()
        {
            #region Variable Declarations
            WpfTable uIFlexgridTable = this.UITestAutomationWindow.UIFlexgridTable;
            #endregion

            // Verify that 'flexgrid' table's property 'RowCount' equals '100'
            Assert.AreEqual(this.AssertMethod2ExpectedValues.UIFlexgridTableRowCount, uIFlexgridTable.RowCount);
        }
        
        /// <summary>
        /// AssertMethod3 - Use 'AssertMethod3ExpectedValues' to pass parameters into this method.
        /// </summary>
        public void AssertMethod3()
        {
            #region Variable Declarations
            WpfCheckBox uIAutoGenerateColumnsCheckBox = this.UIAutomationC1WPFFlexGWindow.UIAutoGenerateColumnsCheckBox;
            #endregion

            // Verify that 'AutoGenerateColumns' check box's property 'AutomationId' equals 'chk_AutoGenerateColumns'
            Assert.AreEqual(this.AssertMethod3ExpectedValues.UIAutoGenerateColumnsCheckBoxAutomationId, uIAutoGenerateColumnsCheckBox.AutomationId);
        }
        
        /// <summary>
        /// AssertMethod4 - Use 'AssertMethod4ExpectedValues' to pass parameters into this method.
        /// </summary>
        public void AssertMethod4()
        {
            #region Variable Declarations
            WpfTable uIFlexgridTable = this.UIAutomationC1WPFFlexGWindow.UIFlexgridTable;
            #endregion

            // Verify that 'flexgrid' table's property 'AutomationId' equals 'flexgrid'
            Assert.AreEqual(this.AssertMethod4ExpectedValues.UIFlexgridTableAutomationId, uIFlexgridTable.AutomationId);
        }
        
        #region Properties
        public virtual AssertMethod1ExpectedValues AssertMethod1ExpectedValues
        {
            get
            {
                if ((this.mAssertMethod1ExpectedValues == null))
                {
                    this.mAssertMethod1ExpectedValues = new AssertMethod1ExpectedValues();
                }
                return this.mAssertMethod1ExpectedValues;
            }
        }
        
        public virtual AssertMethod2ExpectedValues AssertMethod2ExpectedValues
        {
            get
            {
                if ((this.mAssertMethod2ExpectedValues == null))
                {
                    this.mAssertMethod2ExpectedValues = new AssertMethod2ExpectedValues();
                }
                return this.mAssertMethod2ExpectedValues;
            }
        }
        
        public virtual AssertMethod3ExpectedValues AssertMethod3ExpectedValues
        {
            get
            {
                if ((this.mAssertMethod3ExpectedValues == null))
                {
                    this.mAssertMethod3ExpectedValues = new AssertMethod3ExpectedValues();
                }
                return this.mAssertMethod3ExpectedValues;
            }
        }
        
        public virtual AssertMethod4ExpectedValues AssertMethod4ExpectedValues
        {
            get
            {
                if ((this.mAssertMethod4ExpectedValues == null))
                {
                    this.mAssertMethod4ExpectedValues = new AssertMethod4ExpectedValues();
                }
                return this.mAssertMethod4ExpectedValues;
            }
        }
        
        public UITestAutomationWindow UITestAutomationWindow
        {
            get
            {
                if ((this.mUITestAutomationWindow == null))
                {
                    this.mUITestAutomationWindow = new UITestAutomationWindow();
                }
                return this.mUITestAutomationWindow;
            }
        }
        
        public UIAutomationC1WPFFlexGWindow UIAutomationC1WPFFlexGWindow
        {
            get
            {
                if ((this.mUIAutomationC1WPFFlexGWindow == null))
                {
                    this.mUIAutomationC1WPFFlexGWindow = new UIAutomationC1WPFFlexGWindow();
                }
                return this.mUIAutomationC1WPFFlexGWindow;
            }
        }
        #endregion
        
        #region Fields
        private AssertMethod1ExpectedValues mAssertMethod1ExpectedValues;
        
        private AssertMethod2ExpectedValues mAssertMethod2ExpectedValues;
        
        private AssertMethod3ExpectedValues mAssertMethod3ExpectedValues;
        
        private AssertMethod4ExpectedValues mAssertMethod4ExpectedValues;
        
        private UITestAutomationWindow mUITestAutomationWindow;
        
        private UIAutomationC1WPFFlexGWindow mUIAutomationC1WPFFlexGWindow;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'AssertMethod1'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class AssertMethod1ExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that 'Unknown Name' check box's property 'ControlType' equals 'CheckBox'
        /// </summary>
        public string UIItemCheckBoxControlType = "CheckBox";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'AssertMethod2'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class AssertMethod2ExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that 'flexgrid' table's property 'RowCount' equals '100'
        /// </summary>
        public int UIFlexgridTableRowCount = 100;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'AssertMethod3'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class AssertMethod3ExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that 'AutoGenerateColumns' check box's property 'AutomationId' equals 'chk_AutoGenerateColumns'
        /// </summary>
        public string UIAutoGenerateColumnsCheckBoxAutomationId = "chk_AutoGenerateColumns";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'AssertMethod4'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class AssertMethod4ExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that 'flexgrid' table's property 'AutomationId' equals 'flexgrid'
        /// </summary>
        public string UIFlexgridTableAutomationId = "flexgrid";
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class UITestAutomationWindow : WpfWindow
    {
        
        public UITestAutomationWindow()
        {
            #region Search Criteria
            this.SearchProperties[WpfWindow.PropertyNames.Name] = "Test Automation";
            this.SearchProperties.Add(new PropertyExpression(WpfWindow.PropertyNames.ClassName, "HwndWrapper", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("Test Automation");
            #endregion
        }
        
        #region Properties
        public UIFlexgridTable UIFlexgridTable
        {
            get
            {
                if ((this.mUIFlexgridTable == null))
                {
                    this.mUIFlexgridTable = new UIFlexgridTable(this);
                }
                return this.mUIFlexgridTable;
            }
        }
        #endregion
        
        #region Fields
        private UIFlexgridTable mUIFlexgridTable;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class UIFlexgridTable : WpfTable
    {
        
        public UIFlexgridTable(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WpfTable.PropertyNames.AutomationId] = "flexgrid";
            this.WindowTitles.Add("Test Automation");
            #endregion
        }
        
        #region Properties
        public UIRow0DataItem UIRow0DataItem
        {
            get
            {
                if ((this.mUIRow0DataItem == null))
                {
                    this.mUIRow0DataItem = new UIRow0DataItem(this);
                }
                return this.mUIRow0DataItem;
            }
        }
        #endregion
        
        #region Fields
        private UIRow0DataItem mUIRow0DataItem;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class UIRow0DataItem : WpfControl
    {
        
        public UIRow0DataItem(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.ControlType] = "DataItem";
            this.SearchProperties[UITestControl.PropertyNames.Name] = "Row 0";
            this.WindowTitles.Add("Test Automation");
            #endregion
        }
        
        #region Properties
        public UIItem01DataItem UIItem01DataItem
        {
            get
            {
                if ((this.mUIItem01DataItem == null))
                {
                    this.mUIItem01DataItem = new UIItem01DataItem(this);
                }
                return this.mUIItem01DataItem;
            }
        }
        #endregion
        
        #region Fields
        private UIItem01DataItem mUIItem01DataItem;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class UIItem01DataItem : WpfControl
    {
        
        public UIItem01DataItem(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.ControlType] = "DataItem";
            this.SearchProperties[UITestControl.PropertyNames.Name] = "0,1";
            this.WindowTitles.Add("Test Automation");
            #endregion
        }
        
        #region Properties
        public WpfCheckBox UIItemCheckBox
        {
            get
            {
                if ((this.mUIItemCheckBox == null))
                {
                    this.mUIItemCheckBox = new WpfCheckBox(this);
                    #region Search Criteria
                    this.mUIItemCheckBox.WindowTitles.Add("Test Automation");
                    #endregion
                }
                return this.mUIItemCheckBox;
            }
        }
        #endregion
        
        #region Fields
        private WpfCheckBox mUIItemCheckBox;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class UIAutomationC1WPFFlexGWindow : WpfWindow
    {
        
        public UIAutomationC1WPFFlexGWindow()
        {
            #region Search Criteria
            this.SearchProperties[WpfWindow.PropertyNames.Name] = "Automation C1.WPF.FlexGrid.4, Version=4.0.20141.384, Culture=neutral, PublicKeyTo" +
                "ken=2aa4ec5576d6c3ce";
            this.SearchProperties.Add(new PropertyExpression(WpfWindow.PropertyNames.ClassName, "HwndWrapper", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("Automation C1.WPF.FlexGrid.4, Version=4.0.20141.384, Culture=neutral, PublicKeyTo" +
                    "ken=2aa4ec5576d6c3ce");
            #endregion
        }
        
        #region Properties
        public WpfCheckBox UIAutoGenerateColumnsCheckBox
        {
            get
            {
                if ((this.mUIAutoGenerateColumnsCheckBox == null))
                {
                    this.mUIAutoGenerateColumnsCheckBox = new WpfCheckBox(this);
                    #region Search Criteria
                    this.mUIAutoGenerateColumnsCheckBox.SearchProperties[WpfCheckBox.PropertyNames.AutomationId] = "chk_AutoGenerateColumns";
                    this.mUIAutoGenerateColumnsCheckBox.WindowTitles.Add("Automation C1.WPF.FlexGrid.4, Version=4.0.20141.384, Culture=neutral, PublicKeyTo" +
                            "ken=2aa4ec5576d6c3ce");
                    #endregion
                }
                return this.mUIAutoGenerateColumnsCheckBox;
            }
        }
        
        public WpfTable UIFlexgridTable
        {
            get
            {
                if ((this.mUIFlexgridTable == null))
                {
                    this.mUIFlexgridTable = new WpfTable(this);
                    #region Search Criteria
                    this.mUIFlexgridTable.SearchProperties[WpfTable.PropertyNames.AutomationId] = "flexgrid";
                    this.mUIFlexgridTable.WindowTitles.Add("Automation C1.WPF.FlexGrid.4, Version=4.0.20141.384, Culture=neutral, PublicKeyTo" +
                            "ken=2aa4ec5576d6c3ce");
                    #endregion
                }
                return this.mUIFlexgridTable;
            }
        }
        #endregion
        
        #region Fields
        private WpfCheckBox mUIAutoGenerateColumnsCheckBox;
        
        private WpfTable mUIFlexgridTable;
        #endregion
    }
}
