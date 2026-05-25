using C1.WPF.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

#pragma warning disable 1591

namespace DiagramExplorer.Data
{
    public class TypeInfo
    {
        Type type;
        List<TypeInfo> list = new List<TypeInfo>();
        List<string> properties = new List<string>();
        List<string> events = new List<string>();
        List<string> methods = new List<string>();

        public TypeInfo(Type type, Assembly[]? assemblies = null)
        {
            this.type = type;

            var assembly = type.Assembly;

            var types = assembly.GetTypes().Where(t => t.BaseType == (type));

            foreach (var t in types)
                list.Add(new TypeInfo(t));

            if (assemblies != null)
            {
                foreach (var a in assemblies)
                {
                    types = a.GetTypes().Where(t => t.BaseType == (type));
                    foreach (var t in types)
                        list.Add(new TypeInfo(t));
                }
            }

            var props = type.GetProperties( System.Reflection.BindingFlags.DeclaredOnly | 
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var prop in props)
                properties.Add(prop.Name);
            properties.Sort();

            var evs = type.GetEvents(System.Reflection.BindingFlags.DeclaredOnly |
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var ev in evs)
                events.Add(ev.Name);
            events.Sort();

            var ms = type.GetMethods(System.Reflection.BindingFlags.DeclaredOnly |
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var m in ms)
            {
                if (!m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && 
                    !m.Name.StartsWith("add_") && !m.Name.StartsWith("remove_"))
                    methods.Add(m.Name);
            }
            methods.Sort();
        }

        public Type Type => type;

        public List<TypeInfo> Childs => list;

        public List<string> Properties => properties;

        public bool HasProperties => properties.Count > 0;

        public List<string> Events => events;

        public bool HasEvents => events.Count > 0;

        public List<string> Methods => methods;

        public bool HasMethods => methods.Count > 0;


        public override string ToString() => type.Name;
    }
}
