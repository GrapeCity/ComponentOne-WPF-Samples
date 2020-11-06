using C1.WPF.ExpressionEditor;
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

namespace C1ExpressionEditorSample
{
    /// <summary>
    /// A UserControl which contains ExpressionEditor, ExpressionEditorPanel, implementation of preview, OK&Cancel buttons.
    /// When OK button is pressed, the expression will be passed to the ExpressionEditor of the appointed Column to do the Calculation, Filter, Grouping.
    /// </summary>
    public partial class ExpressionEditorEE : UserControl
    {
        //---------------------
        #region ** events
        public event EventHandler OkClick;
        public event EventHandler CancelClick;
        /// <summary>
        /// Occurs when expression string changed.
        /// </summary>
        public event EventHandler ExpressionChanged;
        /// <summary>
        /// Rises the <see cref="ExpressionChanged"/> event.
        /// </summary>
        protected void OnExpressionChanged(EventArgs e)
        {
            if (ExpressionChanged != null)
                ExpressionChanged(this, e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CancelClick?.Invoke(sender, e);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            OkClick?.Invoke(sender, e);
        }
        #endregion

        //---------------------
        #region ** dependency properties

        public string OKContent
        {
            get { return (string)GetValue(OKContentProperty); }
            set
            {
                SetValue(OKContentProperty, value);
            }
        }
        public static readonly DependencyProperty OKContentProperty =
            DependencyProperty.Register(
                "OKContent",
                typeof(string),
                typeof(ExpressionEditorEE),
                new PropertyMetadata("OK")
                );

        public string CancelContent
        {
            get { return (string)GetValue(CancelContentProperty); }
            set
            {
                SetValue(CancelContentProperty, value);
            }
        }
        public static readonly DependencyProperty CancelContentProperty =
            DependencyProperty.Register(
                "CancelContent",
                typeof(string),
                typeof(ExpressionEditorEE),
                new PropertyMetadata("Cancel")
                );

        public Visibility OperateVisibility
        {
            get { return (Visibility)GetValue(OperateVisibilityProperty); }
            set
            {
                SetValue(OperateVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty OperateVisibilityProperty =
            DependencyProperty.Register(
                "OperateVisibility",
                typeof(Visibility),
                typeof(ExpressionEditorEE),
                new PropertyMetadata(Visibility.Visible)
                );
        #endregion

        //---------------------
        #region ** ctor

        public ExpressionEditorEE()
        {
            InitializeComponent();
        }
        #endregion

        //---------------------
        #region ** public interface
        /// <summary>
        /// Gets or sets the object used as the data source.
        /// </summary>
        public object DataSource
        {
            get
            {
                return this.expressionEditor.DataSource;
            }
            set
            {
                this.expressionEditor.DataSource = value;
            }
        }

        /// <summary>
        /// Gets or sets expression string.
        /// </summary>
        public string Expression
        {
            get
            {
                return this.expressionEditor.Expression;
            }
            set
            {
                this.expressionEditor.Expression = value;
            }
        }

        /// <summary>
        /// Gets value that indicates whether the expression is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.expressionEditor.IsValid;
            }
        }
        #endregion

        //---------------------
        #region ** implementation
        private void ExpressionEditor_ExpressionChanged(object sender, EventArgs e)
        {
            errors.Text = "";
            if (!IsValid)
            {
                result.Text = "";
                this.btn_Ok.IsEnabled = false;
                var editError = this.expressionEditor.GetErrors().FirstOrDefault();
                errors.Text = editError?.FullMessage;
            }
            else
            {
                result.Text = this.expressionEditor.Evaluate()?.ToString();
                this.btn_Ok.IsEnabled = true;
                OnExpressionChanged(e);
            }
        }
        #endregion
    }
}
