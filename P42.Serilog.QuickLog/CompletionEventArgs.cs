using System;
using System.Runtime.CompilerServices;

namespace P42.Serilog.QuickLog
{
    public class CompletionEventArgs<T> : QLogEventArgs 
    {

        #region Properties

        public Task CompletedAsync() => tcs.Task;

        public bool IsComplete => tcs.Task.IsCompleted;
        #endregion


        #region Fields
        protected TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
        #endregion


        #region Construction
        public CompletionEventArgs(LogLevel level, string title, string message, string callerClass, string callerMethod, int lineNumber) :
            base(level, null, title, message, callerClass, callerMethod, lineNumber)
        {
        }
        #endregion


        bool completed;
        protected virtual bool InnerComplete()
        {
            if (completed)
                return true;
            completed = true;
            return false;
        }
    }

}