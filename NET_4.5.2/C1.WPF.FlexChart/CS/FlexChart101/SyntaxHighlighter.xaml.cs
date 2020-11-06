using C1.WPF.RichTextBox;
using C1.WPF.RichTextBox.Documents;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexChart101
{
    /// <summary>
    /// Interaction logic for SyntaxHighlighter.xaml
    /// </summary>
    public partial class SyntaxHighlighter : UserControl
    {
        C1RangeStyleCollection _rangeStyles = new C1RangeStyleCollection();

        // initialize regular expression used to parse xaml
        string tagPattern =
            @"</?(?<tagName>[a-zA-Z0-9_:\-]+)" +
            @"(\s+(?<attName>[a-zA-Z0-9_:\-]+)(?<attValue>(=""[^""]+"")?))*\s*/?>";
        // initialize regular expression used to parse csharp
        string stringPattern = @"""(?:\\""|.)*?""";
        string commentPattern = @"//[^\n]*\n|/\*(.|[\r\n])*?\*/";

        // reserved words kept to a minimum (not complete)
        string[] reservedWords = new string[] { "if", "else", "public", "int", "double", "string", "bool", "using", "class", "object", "public", "private", "partial", "void", "namespace", "for", "foreach" };

        // initialize styles used to color the document
        C1TextElementStyle brBlue = new C1TextElementStyle 
        { 
            { C1TextElement.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)) }
        };
        C1TextElementStyle brDarkBlue = new C1TextElementStyle 
        { 
            { C1TextElement.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 0, 0, 180)) }
        };
        C1TextElementStyle brDarkRed = new C1TextElementStyle 
        { 
            { C1TextElement.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 180, 0, 0)) }
        };
        C1TextElementStyle brLightRed = new C1TextElementStyle 
        { 
            { C1TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red) }
        };
        C1TextElementStyle brGreen = new C1TextElementStyle 
        { 
            { C1TextElement.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 0, 160, 0)) }
        };

        public SyntaxHighlighter()
        {
            this.InitializeComponent();
        }

        // perform syntax coloring
        void UpdateSyntaxColoring()
        {
            C1TextRange range = c1RichTextBox1.Document.ContentRange;

            // reset coloring
            _rangeStyles = new C1RangeStyleCollection();
            c1RichTextBox1.StyleOverrides.Clear();
            c1RichTextBox1.StyleOverrides.Add(_rangeStyles);

            var input = range.Text;

            if (Mode == SyntaxMode.Xaml)
            {
                // highlight the matches            
                foreach (Match m in Regex.Matches(input, tagPattern))
                {
                    // select whole tag, make it dark blue
                    _rangeStyles.Add(new C1RangeStyle(GetRangeAtTextOffset(range.Start, m), brDarkBlue));

                    // select tag name, make it dark red
                    var tagName = m.Groups["tagName"];
                    _rangeStyles.Add(new C1RangeStyle(GetRangeAtTextOffset(range.Start, tagName), brDarkRed));

                    // select attribute names, make them light red
                    var attGroup = m.Groups["attName"];
                    if (attGroup != null)
                    {
                        var atts = attGroup.Captures;
                        for (int i = 0; i < atts.Count; i++)
                        {
                            var att = atts[i];
                            _rangeStyles.Add(new C1RangeStyle(GetRangeAtTextOffset(range.Start, att), brLightRed));
                        }
                    }
                }
            }
            else if (Mode == SyntaxMode.CSharp)
            {
                // highlight reserved words first
                foreach (string keyword in reservedWords)
                {
                    foreach (Match m in Regex.Matches(input, keyword + " "))
                    {
                        // select reserved word, make it blue
                        _rangeStyles.Add(new C1RangeStyle(GetRangeAtTextOffset(range.Start, m), brBlue));
                    }
                }

                // highlight strings            
                foreach (Match m in Regex.Matches(input, stringPattern))
                {
                    // select string, make it red
                    _rangeStyles.Add(new C1RangeStyle(GetRangeAtTextOffset(range.Start, m), brDarkRed));
                }

                // highlight comments           
                foreach (Match m in Regex.Matches(input, commentPattern))
                {
                    // select comment, make it green
                    _rangeStyles.Add(new C1RangeStyle(GetRangeAtTextOffset(range.Start, m), brGreen));
                }
            }
        }

        C1TextRange GetRangeAtTextOffset(C1TextPointer pos, Capture capture)
        {
            var start = pos.GetPositionAtOffset(capture.Index, C1TextRange.TextTagFilter);
            var end = start.GetPositionAtOffset(capture.Length, C1TextRange.TextTagFilter);
            return new C1TextRange(start, end);
        }

        #region Mode
        public SyntaxMode Mode { get; set; }
        public enum SyntaxMode { Xaml, CSharp }
        #endregion

        #region Text
        private static string DefaultText = "No text found";

        /// <summary>
        /// IsAvailable Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(SyntaxHighlighter),
                new PropertyMetadata(DefaultText, OnTextChanged));

        /// <summary>
        /// Gets or sets the Text property. This dependency property
        /// indicates ....
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsAvailable property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (SyntaxHighlighter)d;
            string oldText = (string)e.OldValue;
            string newText = target.Text;
            target.OnTextChanged(oldText, newText);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the IsAvailable property.
        /// </summary>
        /// <param name="oldIsAvailable">The old IsAvailable value</param>
        /// <param name="newIsAvailable">The new IsAvailable value</param>
        protected virtual void OnTextChanged(string oldText, string newText)
        {
            c1RichTextBox1.Text = newText;
            UpdateSyntaxColoring();
            c1RichTextBox1.VerticalOffset = 0.0;
        }
        #endregion
    }
}
