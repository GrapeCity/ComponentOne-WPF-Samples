using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Data;
using System.IO;

namespace Word.Creator
{
    public static class DataAccess
    {
        public static Stream GetStream(string resName)
        {
            var asm = typeof(DataAccess).Assembly;
            foreach (var res in asm.GetManifestResourceNames())
            {
                if (res.EndsWith(resName))
                {
                    return asm.GetManifestResourceStream(res);
                }
            }
            Debug.Assert(false, "not found resource: " + resName);
            return null;
        }

        public static DataSet GetDataSet()
        {
            var ds = new DataSet();
            using (var s = GetStream("nwind.zip"))
            {
                var zip = new C1.Zip.C1ZipFile(s);
                using (var zr = zip.Entries[0].OpenReader())
                {
                    ds.ReadXml(zr);
                }
            }
            return ds;
        }

    }
}
