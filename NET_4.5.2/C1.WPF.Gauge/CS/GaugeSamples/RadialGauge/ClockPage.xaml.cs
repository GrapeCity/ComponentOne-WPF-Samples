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
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;

namespace GaugeSamples
{
    /// <summary>
    /// Interaction logic for ClockPage.xaml
    /// </summary>
    public partial class ClockPage : UserControl
    {
        private delegate void UpdateUIDelegate();
        private Timer _timer;

        public ClockPage()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Thread thread = new Thread(new ThreadStart(RunClock));
                thread.Start();
            }
        }

        private void RunClock()
        {
            _timer = new Timer(
                new TimerCallback((target) =>
                {
                    var action = new UpdateUIDelegate(UpdateClock);
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
                }),
                null,
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(1))
                ;
        }

        private void UpdateClock()
        {
            var now = DateTime.Now;
            clockHours.Value = now.Hour % 12 + now.Minute / 60.0;
            clockMins.Value = now.Minute + now.Second / 60.0;
            clockSecs.Value = now.Second;
        }
    }
}
