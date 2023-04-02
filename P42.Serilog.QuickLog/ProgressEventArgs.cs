using System;
using System.Runtime.CompilerServices;

namespace P42.Serilog.QuickLog
{
    public class ProgressEventArgs : QLogEventArgs
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
                    ToStringSuppliment = (value * 100).ToString("D") + "%";
                    PercentChanged?.Invoke(this, value);
                    QLog.Log(this);
                    if (value >= 1)
                        Complete();
                }
            }
        }

        public Task CompletedAsync() => tcs.Task;

        public bool IsComplete => tcs.Task.IsCompleted;
        #endregion


        #region Events
        public event EventHandler<double> PercentChanged;
        #endregion


        #region Fields
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        #endregion


        #region Construction
        public ProgressEventArgs(string title, string message, string callerClass, string callerMethod, int lineNumber) :
            base(LogLevel.Progress, null, title, message, callerClass, callerMethod, lineNumber)
        {
        }
        #endregion

        bool completed;
        public void Complete()
        {
            if (completed)
                return;
            completed = true;
            Progress = 1;
            tcs.SetResult(true);
        }

    }

}