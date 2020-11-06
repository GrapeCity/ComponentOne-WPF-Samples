using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.IO;

namespace GapAnalysis
{
    public class FormModel
    {
        ICollectionView _fields;

        public FormModel()
        {
            var cvs = new CollectionViewSource();
            cvs.Source = LoadFields();
            _fields = cvs.View;
            _fields.GroupDescriptions.Add(new PropertyGroupDescription("Section"));
            UpdateFieldValues();
        }
        public ICollectionView Fields
        {
            get { return _fields; }
        }
        public void UpdateFieldValues()
        {
            foreach (FormField f in Fields)
            {
                if (!string.IsNullOrEmpty(f.Formula))
                {
                    var value = Evaluate(f.Formula);
                    try
                    {
                        f.Value = Evaluate(f.Formula);
                    }
                    catch { }
                }
            }
        }
        double Evaluate(string formula)
        {
            double value = 0;

            // sums
            var arr = SplitString(formula, '+');
            if (arr != null)
            {
                return Math.Max(0, Evaluate(arr[0]) + Evaluate(arr[1]));
            }

            // subractions
            arr = SplitString(formula, '-');
            if (arr != null)
            {
                return Math.Max(0, Evaluate(arr[0]) - Evaluate(arr[1]));
            }

            // ranges
            arr = SplitString(formula, ':');
            if (arr != null)
            {
                bool keep = false;
                var first = GetField(arr[0]);
                var last = GetField(arr[1]);
                foreach (FormField f in this.Fields)
                {
                    if (f == first)
                    {
                        keep = true;
                    }
                    if (keep && f.Value is double)
                    {
                        value += (double)f.Value;
                    }
                    if (f == last)
                    {
                        break;
                    }
                }
                return Math.Max(0, value);
            }

            // direct references
            var field = GetField(formula);
            return field == null || field.Value == null ? 0 : (double)field.Value;
        }
        string[] SplitString(string text, char sep)
        {
            var index = text.IndexOf(sep);
            if (index > -1)
            {
                return new string[] 
                {
                    text.Substring(0, index),
                    text.Substring(index + 1, text.Length - index - 1)
                };
            }
            return null;
        }
        FormField GetField(string cellRef)
        {
            foreach (FormField f in Fields)
            {
                if (f.ID == cellRef)
                    return f;
            }
            return null;
        }

        List<FormField> LoadFields()
        {
            var list = new List<FormField>();
            var section = string.Empty;
            using (var sr = new StreamReader(GetResourceStream()))
            {
                for (var line = sr.ReadLine(); line != null; line = sr.ReadLine())
                {
                    var arr = line.Split('\t');

                    // skip separators
                    if (arr.Length == 0)
                    {
                        continue;
                    }

                    // keep track of section
                    if (arr.Length == 1)
                    {
                        section = arr[0];
                        continue;
                    }

                    // add form field
                    if (arr.Length == 2 || arr.Length == 3)
                    {
                        var formula = arr.Length == 3 ? arr[2] : null;
                        list.Add(new FormField(this, arr[0], section, arr[1], formula));
                    }
                }
            }
            return list;
        }
        Stream GetResourceStream()
        {
            var asm = this.GetType().Assembly;
            foreach (var fn in asm.GetManifestResourceNames())
            {
                if (fn.EndsWith("FormFields.txt"))
                {
                    return asm.GetManifestResourceStream(fn);
                }
            }
            return null;
        }
    }
}
