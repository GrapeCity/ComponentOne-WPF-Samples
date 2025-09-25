using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Threading;
using System;
using System.IO;



namespace UITesting
{
    [TestFixture]
    public class CodedUITest1: DriverConfiguration
    {
       


        /// <summary>
        /// Verifies that enabling the AutoGenerateColumns checkbox results in the FlexGrid displaying data rows.
        /// </summary>
        [Test]        
        public void Test_WhenAutoGenerateColumnsEnabled_ShouldDisplayDataInFlexGrid()
        {
            // Arrange
            var autoGenCheckbox = DesktopSession.FindElementByAccessibilityId("chk_AutoGenerateColumns");
            if (autoGenCheckbox.GetAttribute("Toggle.ToggleState") != "1")
            {
                autoGenCheckbox.Click();
                Thread.Sleep(1000); // Wait for UI to update
            }

            // Act
            var flexGrid = DesktopSession.FindElementByAccessibilityId("flexgrid");
            var rowsWithText = flexGrid.FindElementsByXPath(".//*")
                .Where(e => !string.IsNullOrWhiteSpace(e.Text))
                .ToList();

            // Assert
            Assert.That(rowsWithText.Count, Is.GreaterThan(0), "Expected at least one row in FlexGrid.");
        }

        /// <summary>
        /// Ensures that clicking the Add Row button increases the number of populated rows in the FlexGrid.
        /// </summary>
        [Test]        
        public void Test_WhenRowIsAdded_ShouldIncreaseFlexGridRowCount()
        {
            // Arrange
            var wait = new WebDriverWait(DesktopSession, TimeSpan.FromSeconds(10));
            var flexGrid = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("flexgrid"));
            int rowsBefore = flexGrid.FindElementsByXPath(".//*")
                                     .Count(e => !string.IsNullOrWhiteSpace(e.Text));

            // Act
            var addRowButton = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("btn_AddRow"));
            addRowButton.Click();
            flexGrid.Click(); // Ensure UI updates are captured

            bool rowAdded = wait.Until(drv =>
            {
                int currentCount = flexGrid.FindElementsByXPath(".//*")
                                           .Count(e => !string.IsNullOrWhiteSpace(e.Text));
                return currentCount > rowsBefore;
            });

            int rowsAfter = flexGrid.FindElementsByXPath(".//*")
                        .Count(e => !string.IsNullOrWhiteSpace(e.Text));

            // Assert
            Assert.That(rowsAfter, Is.GreaterThan(rowsBefore), "Expected row count to increase after adding.");
        }

        /// <summary>
        /// Validates that clicking the Remove Row button results in fewer populated rows in the FlexGrid.
        /// </summary>
        [Test]
        public void Test_WhenRowIsRemoved_ShouldDecreaseFlexGridRowCount()
        {
            // Arrange
            var wait = new WebDriverWait(DesktopSession, TimeSpan.FromSeconds(10));
            var flexGrid = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("flexgrid"));
            int rowsBefore = flexGrid.FindElementsByXPath(".//*")
                                     .Count(e => !string.IsNullOrWhiteSpace(e.Text));

            // Act
            var removeRowButton = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("btn_RemoveRow"));
            removeRowButton.Click();
            flexGrid.Click();           

            int rowsAfter = flexGrid.FindElementsByXPath(".//*")
                        .Count(e => !string.IsNullOrWhiteSpace(e.Text));

            // Assert
            Assert.That(rowsAfter, Is.LessThan(rowsBefore), "Expected row count to decrease after removal.");
        }

        /// <summary>
        /// Confirms that clicking the SelectedIndex button displays a valid index value (≤ 9) in the textbox.
        /// </summary>
        [Test]
        public void Test_WhenSelectedIndexButtonClicked_ShouldDisplayValidIndex()
        {
            // Arrange
            var wait = new WebDriverWait(DesktopSession, TimeSpan.FromSeconds(10));
            var flexGrid = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("flexgrid"));
            flexGrid.Click();

            // Act
            var selectedIndexButton = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("btn_SelectedIndex"));
            selectedIndexButton.Click();
            var selectedIndexTextBox = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("TextBox"));
            string indexText = selectedIndexTextBox.Text;

            // Assert
            Assert.That(int.TryParse(indexText, out int indexValue) && indexValue <= 9,
                        $"Expected a valid index value ≤ 9, but got '{indexText}'.");
        }

        /// <summary>
        /// Verifies that clicking the Copy button populates the textbox with content from the selected row.
        /// </summary>
        [Test]
        public void Test_WhenCopyButtonClicked_ShouldPopulateTextBoxWithRowText()
        {
            // Arrange
            var wait = new WebDriverWait(DesktopSession, TimeSpan.FromSeconds(10));
            var flexGrid = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("flexgrid"));
            flexGrid.Click();

            // Act
            var copyButton = wait.Until(drv => DesktopSession.FindElementByAccessibilityId("btn_Copy"));
            Assert.That(copyButton.Enabled, Is.True, "Copy button should be enabled.");
            copyButton.Click();

            string copiedText = wait.Until(drv =>
                DesktopSession.FindElementByAccessibilityId("TextBox").Text);

            // Assert
            Assert.That(copiedText, Is.Not.Null.And.Not.Empty, "Expected copied text to be present in the textbox.");
        }
    }
}