﻿using C1.WPF.Core;
using C1.WPF.Ribbon;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for ConditionalFormatting.xaml
    /// </summary>
    public partial class ConditionalFormatting : UserControl
    {
        bool isLoaded = false;

        public ConditionalFormatting()
        {
            InitializeComponent();
            Tag = Properties.Resources.ConditionalFormattingDesc;

            flexPivotPage.Loaded += (s, ea) =>
            {
                if (!flexPivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                var fpEngine = flexPivotPage.FlexPivotPanel.C1PivotEngine;
                // apply update to view
                fpEngine.BeginUpdate();
                fpEngine.DataSource = Utils.PivotDataSet.Tables[0].DefaultView;
                // set value field limit to 4 (**default is 1)
                fpEngine.ValueFields.MaxItems = 4;

                // create conditional formatting
                fpEngine.Fields["ExtendedPrice"].StyleHigh.ForeColor = System.Drawing.Color.Green;
                fpEngine.Fields["ExtendedPrice"].StyleHigh.ConditionType = C1.PivotEngine.ConditionType.Percentage;
                fpEngine.Fields["ExtendedPrice"].StyleHigh.Value = 0.8;
                fpEngine.Fields["ExtendedPrice"].StyleLow.ForeColor = System.Drawing.Color.Red;
                fpEngine.Fields["ExtendedPrice"].StyleLow.ConditionType = C1.PivotEngine.ConditionType.Percentage;
                fpEngine.Fields["ExtendedPrice"].StyleLow.Value = 0.1;

                fpEngine.Fields["ExtendedPrice"].Caption = "Sales";
                fpEngine.RowFields.Add("Country");
                fpEngine.ColumnFields.Add("Salesperson");
                fpEngine.ValueFields.Add("ExtendedPrice");
                fpEngine.Fields["ExtendedPrice"].Format = "c0";
                fpEngine.EndUpdate();
            };
        }
    }
}
