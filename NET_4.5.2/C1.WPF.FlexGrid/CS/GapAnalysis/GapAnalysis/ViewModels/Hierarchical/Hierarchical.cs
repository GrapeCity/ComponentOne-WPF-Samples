using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;

namespace GapAnalysis
{
    /// <summary>
    /// ViewModel for "Hierarchical" binding tab.
    /// </summary>
    public class Hierarchical
    {
        ICollectionView _files;

        public Hierarchical()
        {
            var cvs = new CollectionViewSource();
            cvs.Source = File.GetFiles(100);
            _files = cvs.View;
        }
        public ICollectionView Files
        {
            get { return _files; }
        }
    }
}
