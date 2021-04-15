using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WSSAT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string XTIP_CONTENT = "(Press any key to {0})";
        public const string XTIP_START_CONTENT = ">: {0}";

        private readonly Stopwatch _sw = new Stopwatch();
        private readonly DispatcherTimer _dtTimer = new DispatcherTimer();

        private bool _doNotDisturb;

        public StopwatchState State { get; private set; } = StopwatchState.None;
        public TimeSpan Elapsed => _sw.Elapsed;

        public MainWindow()
        {
            InitializeComponent();

            //initialize timer
            _dtTimer.Interval = TimeSpan.FromMilliseconds(10.0);
            _dtTimer.Tick += OnTick;

            //hotkey
            this.PreviewKeyDown += OnPreviewKeyDown;
            this.PreviewKeyUp += OnPreviewKeyUp;
        }

        private void xProdigy_Click(object sender, RoutedEventArgs e)
        {
            ActionLikeProdigy();
        }

        /// <summary>
        /// stopwatch button
        /// </summary>
        private void ActionLikeProdigy()
        {
            switch (State)
            {
                case StopwatchState.None:
                    _sw.Start();

                    State = StopwatchState.Recording;
                    _dtTimer.Start();

                    xTipStart.Content = string.Format(XTIP_START_CONTENT, "Stop");
                    xTip.Content = string.Format(XTIP_CONTENT, "stop");

                    break;

                case StopwatchState.Recording:
                    _sw.Stop();
                    State = StopwatchState.Stopped;

                    xTipStart.Content = string.Format(XTIP_START_CONTENT, "Restart");
                    xTip.Content = string.Format(XTIP_CONTENT, "restart");

                    break;

                case StopwatchState.Stopped:
                    _sw.Restart();
                    State = StopwatchState.Recording;

                    xTipStart.Content = string.Format(XTIP_START_CONTENT, "Stop");
                    xTip.Content = string.Format(XTIP_CONTENT, "stop");

                    break;
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            xTime.Content = $"{(int)Elapsed.TotalSeconds:D2}:{Elapsed.Milliseconds:D3}";
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            //for stop
            if (State == StopwatchState.Recording)
            {
                ActionLikeProdigy();
                _doNotDisturb = true;
            }
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (_doNotDisturb)
            {
                _doNotDisturb = false;
                return;
            }

            //for start
            if (State != StopwatchState.Recording) //equals (State == StopwatchState.None || State == StopwatchState.Stopped)
            {
                ActionLikeProdigy();
            }
        }
    }
}
