using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace process_pousse
{
    public class ActiveApplicationPropertyThread
    {
        private bool _stopThread = true;
        private string _appName = "Idle";
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();
        [DllImport("user32")]
        private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);

        public ActiveApplicationInfomationCollector activeApplicationInfomationCollector;
        private string _justSentApplicationName = "";
        public ActiveApplicationPropertyThread()
        {
            activeApplicationInfomationCollector = new ActiveApplicationInfomationCollector();
        }

        public void StopThread()
        {
            _stopThread = false;
        }

        public void StartThread()
        {
            while (_stopThread)
            {
                ActiveAppName();

                if (_justSentApplicationName == "")
                {
                    _justSentApplicationName = _appName;
                    activeApplicationInfomationCollector.ApplicationFocusStart(_appName);
                }
                if (_justSentApplicationName != _appName)
                {
                    activeApplicationInfomationCollector.ApplicationFocusEnd(_justSentApplicationName);
                    activeApplicationInfomationCollector.ApplicationFocusStart(_appName);
                    _justSentApplicationName = _appName;
                }

                Thread.Sleep(2000);
            }
        }

        public string AppInfo
        {
            get { return _appName; }
        }

        private Int32 GetWindowProcessID(Int32 hwnd)
        {
            Int32 pid = 1;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        private void ActiveAppName()
        {
            Int32 hwnd = 0;
            hwnd = GetForegroundWindow();
            try
            {
                _appName = Process.GetProcessById(GetWindowProcessID(hwnd)).ProcessName;
            }
            catch (Exception e)
            {

            }

        }
    }
}
