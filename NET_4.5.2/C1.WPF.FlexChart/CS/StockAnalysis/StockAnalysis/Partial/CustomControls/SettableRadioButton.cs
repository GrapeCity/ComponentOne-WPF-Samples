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
    public class SettableRadioButton : RadioButton
    {
        static SettableRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SettableRadioButton), new FrameworkPropertyMetadata(typeof(SettableRadioButton)));
        }

        public bool IsShowSetting
        {
            get { return (bool)this.GetValue(IsShowSettingProperty); }
            set { this.SetValue(IsShowSettingProperty, value); }
        }
        public static DependencyProperty IsShowSettingProperty = DependencyProperty.Register(
            "IsShowSetting",
            typeof(bool),
            typeof(SettableRadioButton),
            new PropertyMetadata(true)
        );
        
        public Command.SettingCommand SettingCommand
        {
            get { return (Command.SettingCommand)this.GetValue(SettingCommandProperty); }
            set { this.SetValue(SettingCommandProperty, value); }
        }

        public static DependencyProperty SettingCommandProperty = DependencyProperty.Register
            (
                "SettingCommand",
                typeof(Command.SettingCommand),
                typeof(SettableRadioButton),
                new PropertyMetadata(null)
            );

    }
}
