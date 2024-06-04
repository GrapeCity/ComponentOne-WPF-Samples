using C1.WPF.ExpressionEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace C1ExpressionEditorSample
{
    public class ExpressionGroupDescription : GroupDescription
    {
        public static C1ExpressionEditor editer = new C1ExpressionEditor();

        public string Expression
        {
            get;
            set;
        }

        public override object GroupNameFromItem(object item, int level, CultureInfo culture)
        {
            editer.DataSource = item;
            editer.Expression = Expression;

            if (editer.IsValid)
                return editer.Evaluate();
            return "";
        }
    }

    public class C1DataGridExpressionGroupDescription : PropertyGroupDescription
    {
        public static C1ExpressionEditor editer = new C1ExpressionEditor();

        public string Expression
        {
            get;
            set;
        }

        public override object GroupNameFromItem(object item, int level, CultureInfo culture)
        {
            editer.DataSource = item;
            editer.Expression = Expression;

            if (editer.IsValid)
                return editer.Evaluate();
            return "";
        }
    }
}
