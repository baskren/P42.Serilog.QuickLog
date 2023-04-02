using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace P42.Serilog.QuickLog
{
    public class PermissionLogger : CompletionLogger<PermissionState>
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
                    InnerComplete();
                }
            }
        }

        protected override string ToStringSuppliment => $"Permission: [{_state}]";

        public PermissionLogger(string title, string message, string callerClass, string callerMethod, int lineNumber) :
            base(LogLevel.Permission, title, message, callerClass, callerMethod, lineNumber)
        {
        }

        protected override bool InnerComplete()
        {
            if (!base.InnerComplete())
            {
                tcs.SetResult(_state);
                return false;
            }
            return true;

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