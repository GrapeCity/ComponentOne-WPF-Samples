using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF;
using C1.WPF.RichTextBox;
using C1.WPF.SpellChecker;
using C1.WPF.RichTextBox.Documents;

namespace RichTextBoxSamples
{
    public partial class DemoRichTextBoxFilter
        : UserControl
    {
        public DemoRichTextBoxFilter()
        {
            InitializeComponent();

            var spellChecker = new C1SpellChecker();
            richToolbar.SpellChecker = spellChecker;
            spellChecker.MainDictionary.LoadAsync("C1Spell_en-US.dct");

            richTextBox.Html = html;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var oldItem = e.RemovedItems.OfType<C1TabItem>().FirstOrDefault();
                if (oldItem == null) return; // richTextBoxTab and the others are null the first time around because InitializeComponent is running.

                if(oldItem == richTextBoxTab)
                {
                    htmlBox.Text = richTextBox.Html;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if(oldItem == htmlTab)
                {
                    richTextBox.Html = htmlBox.Text;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if(oldItem == rtfTab)
                {
                    richTextBox.Document = new RtfFilter().ConvertToDocument(rtfBox.Text);
                    htmlBox.Text = richTextBox.Html;
                }
            }
            catch {}
        }


        string html =
            @"<html>
                <head>
                    <style>
                        td
                        {
                            border: solid 1px Black;
                            padding: 2px;
                        }
                    </style>
                </head>
                <body style=""font-family: Verdana; font-size: medium;"">
                <p>
                <b>C1RichTextBox</b> supports importing and exporting to <span style='text-decoration: underline'>HTML</span> and <span style='text-decoration: underline'>RTF</span>.
                </p>
                <p style='text-align:center; background-color:#FFFFCC; font-size:9pt'>
                    Switch the tabs below to see this document in HTML and RTF formats.
                </p>
                </body>
            </html>";
    }
}
