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

namespace StockAnalysis.Partial.CustomControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:StockAnalysis.Partial.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:StockAnalysis.Partial.CustomControls;assembly=StockAnalysis.Partial.CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:SettableCheckBox/>
    ///
    /// </summary>
    public class SettableCheckBox : CheckBox
    {
        static SettableCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SettableCheckBox), new FrameworkPropertyMetadata(typeof(SettableCheckBox)));
        }
        public bool IsShowSetting
        {
            get { return (bool)this.GetValue(IsShowSettingProperty); }
            set { this.SetValue(IsShowSettingProperty, value); }
        }
        public static DependencyProperty IsShowSettingProperty = DependencyProperty.Register(
            "IsShowSetting",
            typeof(bool),
            typeof(SettableCheckBox),
            new PropertyMetadata(true)
        );

        public Command.SettingCommand SettingCommand
        {
            get { return (Command.SettingCommand) this.GetValue(SettingCommandProperty); }
            set { this.SetValue(SettingCommandProperty, value); }
        }

        public static DependencyProperty SettingCommandProperty = DependencyProperty.Register
            (
                "SettingCommand",
                typeof(Command.SettingCommand),
                typeof(SettableCheckBox),
                new PropertyMetadata(null)
            );


    }
}
