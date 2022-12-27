using System;
using System.Runtime.CompilerServices;

namespace P42.Serilog.QuickLog
{
    public class ProgressEventArgs : QLogEventArgs
    {

        #region Properties
        int _percent;
        public int Percent
        {
            get => _percent;
            set
            {
                if (_percent != value) 
                { 
                    _percent = value;
                    PercentChanged?.Invoke(this, value);
                    if (value >= 100)
                        Complete();
                }
            }
        }

        public Task CompletedAsync() => tcs.Task;

        public bool IsComplete => tcs.Task.IsCompleted;
        #endregion


        #region Events
        public event EventHandler<int> PercentChanged;
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


        public void Complete()
        {
            Percent = 100;
            tcs.SetResult(true);
        }

        public override string ToString()
        {
            var result = base.ToString();
            result += Percent + "%";
            return result;
        }
    }

}