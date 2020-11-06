using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GapAnalysis
{
    public class File :
        BaseEditableObject
    {
        static string[] _extensions = "doc|xls|docx|xlsx|txt|png|jpg|mp3|mp4|exe|pdf|msi|zip|html".Split('|');
        static string[] _names = "adapter|trust|quick start|reference|help|source|test|demo|images|help|web|browser|compress".Split('|');

        string _name, _extension;
        long _size;
        DateTime _date;
        ObservableCollection<File> _files;

        static int _ctr;

        // object model
        [Display(Name = "Name")]
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value, "Name"); }
        }

        [Display(Name = "Extension")]
        public string Extension
        {
            get { return _extension; }
            set { SetValue(ref _extension, value, "Extension"); }
        }

        [Display(Name = "Size")]
        public long Size
        {
            get { return _size; }
        }

        [Display(Name = "Date")]
        public DateTime Date
        {
            get { return _date; }
        }
        public ObservableCollection<File> Files
        {
            get { return _files; }
        }

        // factory
        public static ObservableCollection<File> GetFiles(int count)
        {
            var list = new ObservableCollection<File>();
            var rnd = new Random(0);

            // create files
            while (count > 0)
            {
                list.Add(CreateFile(rnd, ref count));
            }

            // done
            return list;
        }
        static File CreateFile(Random rnd, ref int count)
        {
            var f = new File();
            count--;
            if (rnd.NextDouble() < 0.8 || count < 10)
            {
                // add file
                f._name = _names[rnd.Next() % _names.Length];
                f._extension = _extensions[rnd.Next() % _extensions.Length];
                f._date = DateTime.Today.AddDays(rnd.Next(-3000, 0));
                f._size = rnd.Next(10, 100000000);
            }
            else
            {
                // add folder
                f._name = _names[rnd.Next() % _names.Length].ToUpper();
                f._files = new ObservableCollection<File>();
                for (int i = 0; i < rnd.Next(1, 10) && count > 0; i++)
                {
                    f.Files.Add(CreateFile(rnd, ref count));
                }

                // compute folder size and date
                f._size = 0;
                f._date = DateTime.MinValue;
                foreach (var ff in f.Files)
                {
                    f._size += ff.Size;
                    if (ff.Date > f._date)
                    {
                        f._date = ff.Date;
                    }
                }
            }
            return f;
        }
    }
}
