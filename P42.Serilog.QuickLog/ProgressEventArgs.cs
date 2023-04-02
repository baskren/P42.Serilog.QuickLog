using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;

namespace P42.Serilog.QuickLog
{
    public class ProgressEventArgs : CompletionEventArgs<bool>
    {

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
                    PercentChanged?.Invoke(this, value);
                    QLog.Log(this);
                    if (value >= 1)
                        InnerComplete();
                }
            }
        }

        protected override string ToStringSuppliment => (_progress * 100).ToString("D") + "%";
        #endregion

        #region Events
        public event EventHandler<double> PercentChanged;
        #endregion


        #region Construction
        public ProgressEventArgs(string title, string message, string callerClass, string callerMethod, int lineNumber) :
            base(LogLevel.Progress, title, message, callerClass, callerMethod, lineNumber)
        {
        }
        #endregion

        public void Complete()
            => InnerComplete();

        protected override bool InnerComplete()
        {
            if (!base.InnerComplete())
            {
                Progress = 1;
                tcs.SetResult(true);
                return false;
            }
            return true;
        }

    }

}