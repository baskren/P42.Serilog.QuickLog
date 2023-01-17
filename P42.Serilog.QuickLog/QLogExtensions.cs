using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog;

namespace P42.Serilog.QuickLog
{
    public static class QLogExtensions
    {
        public static void LogException(Thread t, Exception e)
        {
            QLog.Error(e, $"THREAD: [{t}]");
        }
    }
}