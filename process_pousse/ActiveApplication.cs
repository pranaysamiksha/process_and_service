using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace process_pousse
{
    public class ActiveApplicationInfomationCollector
    {
        //private int _duration = 0;
        //public string ApplicationName{ get; set; }
        //public int Duration { get { return _duration; } set { _duration = value; } }
        //public void UpdateDuration(int duration)
        //{
        //    _duration = _duration + duration;
        //}


        public DateTime _startTime;
        public Dictionary<string, Double> _focusedApplication;
        public ActiveApplicationInfomationCollector()
        {
            _focusedApplication = new Dictionary<string, Double>();
        }
        public void ApplicationFocusStart(string appName)
        {
            _startTime = DateTime.Now;
            if (!_focusedApplication.ContainsKey(appName))
            {
                _focusedApplication.Add(appName, 0);

            }
        }
        public void ApplicationFocusEnd(string appName)
        {
            //end the timer and update the seconds
            TimeSpan elapsed = DateTime.Now - _startTime;
            _focusedApplication[appName] = _focusedApplication[appName] + elapsed.TotalMilliseconds;
        }

    }
}
