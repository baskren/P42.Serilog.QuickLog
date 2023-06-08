
using System;

namespace P42.Serilog.QuickLog
{
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// Does nothing: not sent to Serilog, QLog.Logged not fired
        /// </summary>
        None = 0,
        Verbose = 1,
        Debug = 2,
        Information = 4,
        Warning = 8,
        Error = 16,
        NotImplemented = 32,
        Fatal = 64,
        Permission = 128,
        Progress = 256,
    }
}