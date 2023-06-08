using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace P42.Serilog.QuickLog
{
    public class CompletionLogger<T> : QLogEventArgs
    {

        #region Properties

        public Task CompletedAsync() => tcs.Task;

        public bool IsComplete => tcs.Task.IsCompleted;
        #endregion


        #region Fields
        bool completed;
        protected TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
        #endregion


        #region Construction
        public CompletionLogger(LogLevel level, string title, string message, string callerClass, string callerMethod, int lineNumber) :
            base(level, null, title, message, callerClass, callerMethod, lineNumber)
        {
        }
        #endregion


        protected virtual bool InnerComplete()
        {
            if (completed)
                return true;
            completed = true;
            return false;
        }

    }

}