using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
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

        public static string ExceptionMessageGenerator(this Exception e, int depth = 0)
        {
            if (e is null)
                return string.Empty;

            var prefix = string.Empty;
            for (int i = 0; i < depth; i++)
                prefix = prefix + "INNER ";

            var result = $"{prefix}EXCEPTION: TYPE[{e.GetType()}] MESSAGE:[{e.Message}] : \n{"HELP["+(!string.IsNullOrWhiteSpace(e.HelpLink) ? e.HelpLink + "\n" : null)+"]"} STACK[{e.StackTrace}]\n";
            return result + ExceptionMessageGenerator(e.InnerException, depth + 1);
        }

    }
}