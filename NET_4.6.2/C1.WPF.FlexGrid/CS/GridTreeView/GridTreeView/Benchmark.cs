using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace GridTreeView
{
    /// <summary>
    /// Benchmarking helper class
    /// </summary>
    class Benchmark : IDisposable
    {
        string _msg;
        DateTime _start;
        TextBlock _tb;

        public Benchmark(string msg, TextBlock tb)
        {
            _msg = msg;
            _start = DateTime.Now;
            _tb = tb;
        }
        public void Dispose()
        {
            _tb.Text = string.Format("{0}: {1:n0} ms", _msg, DateTime.Now.Subtract(_start).TotalMilliseconds);
        }
    }
}
