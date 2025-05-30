using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PowerMill_Helper
{
     class WindowMonitor
    {
        private DispatcherTimer _timer;
        private IntPtr _targetHwnd;
        private IntPtr _lastMonitor;

        public Action<IntPtr> OnMonitorChanged = delegate { };

        public WindowMonitor(IntPtr hwnd)
        {
            _targetHwnd = hwnd;
            _lastMonitor = GetCurrentMonitor(hwnd);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500) 
            };
            _timer.Tick += (s, e) => CheckMonitor();
            _timer.Start();
        }

        private void CheckMonitor()
        {
            var currentMonitor = GetCurrentMonitor(_targetHwnd);
            if (currentMonitor != _lastMonitor)
            {
                _lastMonitor = currentMonitor;
                OnMonitorChanged(currentMonitor);
            }
        }

        private IntPtr GetCurrentMonitor(IntPtr hwnd)
        {
            return MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
        }

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        private const uint MONITOR_DEFAULTTONEAREST = 2;
    }
}
