using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace P42.Serilog.QuickLog
{
    public class ProgressLogger : CompletionLogger<bool>
    {

        public static ObservableCollection<ProgressLogger> ActiveLoggers = new();

        #region Properties
        double _progress = -1;
        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value) 
                { 
                    if (value > 1)
                        value = 1;
                    _progress = value;
                    ProgressChanged?.Invoke(this, value);
                    QLog.Log(this);
                    if (value >= 1)
                        InnerComplete();
                }
            }
        }

        protected override string ToStringSupplement
        {
            get
            {
                var result = $"Progress: [{(Progress >= 0 ? Progress.ToString("P0") : "INDETERMINATE")}]";
                return result ;
            }
        }

        #endregion

        #region Events
        public event EventHandler<double> ProgressChanged;
        #endregion


        #region Construction
        public ProgressLogger(string title, string message, string callerClass, string callerMethod, int lineNumber) :
            base(LogLevel.Progress, title, message, callerClass, callerMethod, lineNumber)
        {
            ActiveLoggers.Add(this);
        }
        #endregion

        public void Complete()
            => InnerComplete();

        protected override bool InnerComplete()
        {
            if (!base.InnerComplete())
            {
                Progress = 1;
                ActiveLoggers.Remove(this);
                tcs.SetResult(true);
                return false;
            }
            return true;
        }

    }

}