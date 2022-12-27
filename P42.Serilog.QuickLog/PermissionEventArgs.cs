using System;
using System.Runtime.CompilerServices;

namespace P42.Serilog.QuickLog
{
    public class PermissionEventArgs : ProgressEventArgs
    {
        PermissionState _state = PermissionState.Pending;
        public PermissionState State 
        { 
            get => _state; 
            set
            {
                if (_state != value)
                {
                    _state = value;
                    Complete();
                }
            }
        }

        public PermissionEventArgs(string title, string message, string callerClass, string callerMethod, int lineNumber) :
            base(title, message, callerClass, callerMethod, lineNumber)
        {
            Level = LogLevel.Permission;
        }
    }

    public enum PermissionState
    {
        Pending,
        Cancelled,
        Unknown,
        Denied,
        Granted,
    }
}