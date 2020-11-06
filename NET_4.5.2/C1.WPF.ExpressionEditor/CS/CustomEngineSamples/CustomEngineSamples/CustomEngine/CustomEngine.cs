using C1.ExpressionEditor.Engine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CustomEngineSamples
{
    public class CustomEngine : IEngine
    {
        #region fields
        private const string _codePlaceHolder = "<code>";
        private const string _argsPlaceHolder = "<args>";
        private const string _usingPlaceHolder = "<using>";
        private const string _template = @"
    using System;
    using System.Linq;    
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    <using>

    namespace RoslynCustomEngine
    {
        public class CustomEngine
        {
            public object Evaluate(<args>)
            {
return <code>;
            }
        }
    }";
        
        private SyntaxTree _syntaxTree;
        private SyntaxTree _cashSyntaxTree;
        private CSharpCompilation _cashcompilation;
        private List<ErrorInfo> _errors = new List<ErrorInfo>();
        private Dictionary<string, object> _variabels = new Dictionary<string, object>();
        private Dictionary<string, string> _usings = new Dictionary<string, string>();
        private List<MetadataReference> _references = new List<MetadataReference>();
        private string _expression;
        #endregion

        #region IEngine
        public CultureInfo CultureInfo
        {
            get;
            set;
        }

        public object ItemContext
        {
            get;
            set;
        }

        public object DataSource
        {
            get;
            set;
        }
        
        public string Expression
        {
            get { return _expression; }
            set
            {
                if (_expression != value)
                {
                    _expression = value;
                    Parse();
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return _errors.Count == 0;
            }
        }

        public object Evaluate()
        {
            Parse();
            if (IsValid)
                return Compile(_syntaxTree);
            else
                return null;
        }

        public bool TryEvaluate(out object result)
        {
            result = null;
            try
            {
                Parse();
                if (IsValid)
                    result = Compile(_syntaxTree);
                if (_errors.Count == 0)
                    return true;
            }
            catch
            {
                result = null;
            }
            return false;
        }

        public List<ErrorInfo> GetErrors()
        {
            return _errors;
        }

        public void AddAlias(string field, string alias)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region private

        private void Parse()
        {
            _errors.Clear();
            _variabels.Clear();
            _usings.Clear();
            _references.Clear();

            var template = _template;
            var expr = Expression;
            // fields            
            if (DataSource != null)
            {
                int st = 0;
                while (st < expr.Length && st > -1)
                {
                    st = expr.IndexOf('[', st);
                    if (st > -1)
                    {
                        var end = expr.IndexOf(']', st + 1);
                        if (end == -1)
                            break;
                        var field = expr.Substring(st + 1, end - st - 1);
                        var v = GetValue(field);
                        _variabels.Add(field, v);
                        expr = expr.Replace(string.Format("[{0}]", field), field);
                    }
                }
            }
            if (_variabels.Keys.Count > 0)
            {
                var args = _variabels.OrderBy(x => x.Key).Select(x => string.Format("{0} {1}", GetStringType(x.Value), x.Key));
                template = template.Replace(_argsPlaceHolder, string.Join(",", args));
            }
            else
                template = template.Replace(_argsPlaceHolder, string.Empty);
            // using
            if (ItemContext != null && ItemContext.GetType().IsClass)
            {
                _usings.Add(ItemContext.GetType().Namespace, ItemContext.GetType().Assembly.Location);
            }
            if (_usings.Keys.Count == 0)
                template = template.Replace(_usingPlaceHolder, string.Empty);
            else
            {
                var usingsStr = string.Empty;
                foreach (var item in _usings)
                {
                    _references.Add(MetadataReference.CreateFromFile(item.Value));
                    usingsStr += string.Format("using {0};", item.Key) + Environment.NewLine;
                }
                template = template.Replace(_usingPlaceHolder, usingsStr);
            }
            //**
            expr = expr.Replace("\n", string.Empty);
            _syntaxTree = CSharpSyntaxTree.ParseText(template.Replace(_codePlaceHolder, expr));
            
            var errors = _syntaxTree.GetDiagnostics().Where(x => x.IsWarningAsError || x.Severity == DiagnosticSeverity.Error);
            _errors.AddRange(errors.Select(x => new CustomErrorInfo(x)));            
        }

        private string GetStringType(object obj)
        {
            if (obj is IList)
            {
                var lst = obj as IList;
                return string.Format("IList<{0}>", DetermineItemType(lst).Name);
            }
            return obj.GetType().ToString();
        }

        private object GetValue(string field)
        {            
            if (DataSource is IList)
            {
                var lst = DataSource as IList;
                var itemType = DetermineItemType(lst);
                const BindingFlags bf =
                    BindingFlags.IgnoreCase |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.Static;
                var props = itemType.GetProperties(bf).Select(x => x.Name);
                if (props.Contains(field) && ItemContext != null)
                {
                    return ItemContext.GetType().GetProperty(field).GetValue(ItemContext);
                }
            }
            return field;
        }

        private static Type DetermineItemType(IList list)
        {
            var enumerable_type =
                list.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumerable_type != null)
                return enumerable_type.GetGenericArguments()[0];

            if (list.Count == 0)
                return null;

            return list[0].GetType();
        }

        private object Compile(SyntaxTree syntaxTree)
        {
            if (_cashSyntaxTree == null || (_cashSyntaxTree != null && !_cashSyntaxTree.IsEquivalentTo(syntaxTree)))
            {
                _references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
                _references.Add(MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location));
                _references.Add(MetadataReference.CreateFromFile(typeof(IBindingList).Assembly.Location));

                _cashSyntaxTree = syntaxTree;
                _cashcompilation = CSharpCompilation.Create(
                    "test.dll",
                    syntaxTrees: new[] { syntaxTree },
                    references: _references.ToArray(),
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            }

            using (var ms = new MemoryStream())
            {
                var result = _cashcompilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);
                    _errors.AddRange(failures.Select(x => new CustomErrorInfo(x)));
                    return null;
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType("RoslynCustomEngine.CustomEngine");
                    object obj = Activator.CreateInstance(type);
                    var res = type.InvokeMember("Evaluate",
                       BindingFlags.Default | BindingFlags.InvokeMethod,
                       null,
                       obj,
                       _variabels.OrderBy(x => x.Key).Select(x => x.Value).ToArray());
                    return res;
                }
            }
        }

        #endregion
    }
}
