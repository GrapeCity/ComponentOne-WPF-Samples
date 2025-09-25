using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UITesting
{
    public class DriverConfiguration
    {
        public const string windowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string relativePath = @"..\..\..\FlexGrid_UIATest\bin\Debug";

        private static WindowsDriver<WindowsElement> _desktopSession;

        public static string ApplicationPath =>
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath, "FlexGrid_UIATest.exe"));

        public static WindowsDriver<WindowsElement> DesktopSession => _desktopSession;

        [SetUp]
        public void Setup()
        {
            if (_desktopSession == null)
            {
                var options = new AppiumOptions();
                options.AddAdditionalCapability("app", ApplicationPath);
                options.AddAdditionalCapability("platformName", "Windows");
                options.AddAdditionalCapability("automationName", "Windows");
                options.AddAdditionalCapability("deviceName", "WindowsPC");

                _desktopSession = new WindowsDriver<WindowsElement>(
                    new Uri(windowsApplicationDriverUrl), options, TimeSpan.FromSeconds(30));

                _desktopSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            }
        }

        [TearDown]
        public void TearDown()
        {
            DesktopSession?.Quit();
            _desktopSession = null;
        }
    }
}
