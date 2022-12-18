using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog;

namespace P42.Serilog.QuickLog
{
    public static class QLog
    {
        /// <summary>
        /// Flagged levels will serialize exceptions as JSON (instead of pretty print)
        /// </summary>
        public static LogLevel SerializeExceptionsToJson { get; set; }

        /// <summary>
        /// Flagged levels will serialize line number to log - since this can tend to be an issue with Android and iOS release builds
        /// </summary>
        public static LogLevel AddLineNumber { get; set; } = LogLevel.Fatal | LogLevel.Error;

        /// <summary>
        /// Event called when QLog is created
        /// </summary>
        public static event EventHandler<QLogEventArgs> Logged;

        /// <summary>
        /// Verbose - with Exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Verbose(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
        {
            if (ex != null)
                Log.Verbose(ex, ParceMessage(LogLevel.Verbose, caller, message, method, lineNumber));
            else
                Log.Verbose(ParceMessage(LogLevel.Verbose, caller, message, method, lineNumber));

            Logged?.Invoke(caller, new QLogEventArgs(LogLevel.Verbose, caller, ex, message, method, lineNumber));
        }

        /// <summary>
        /// Verbose without exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Verbose(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Verbose(caller, null, message, method, lineNumber);


        /// <summary>
        /// Debug with exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Debug(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
        {
            if (ex != null)
                Log.Debug(ex, ParceMessage(LogLevel.Debug, caller, message, method, lineNumber));
            else
                Log.Debug(ParceMessage(LogLevel.Debug, caller, message, method, lineNumber));

            Logged?.Invoke(caller, new QLogEventArgs(LogLevel.Debug, caller, ex, message, method, lineNumber));
        }

        /// <summary>
        /// Debug without exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Debug(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Debug(caller, null, message, method, lineNumber);


        /// <summary>
        /// Information with exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Information(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
        {
            if (ex != null)
                Log.Information(ex, ParceMessage(LogLevel.Information, caller, message, method, lineNumber));
            else
                Log.Information(ParceMessage(LogLevel.Information, caller, message, method, lineNumber));

            Logged?.Invoke(caller, new QLogEventArgs(LogLevel.Information, caller, ex, message, method, lineNumber));
        }

        /// <summary>
        /// Information without exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Information(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Information(caller, null, message, method, lineNumber);



        /// <summary>
        /// Warning with exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Warning(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
        {
            if (ex != null)
                Log.Warning(ex, ParceMessage(LogLevel.Warning, caller, message, method, lineNumber));
            else
                Log.Warning(ParceMessage(LogLevel.Warning, caller, message, method, lineNumber));

            Logged?.Invoke(caller, new QLogEventArgs(LogLevel.Warning, caller, ex, message, method, lineNumber));
        }

        /// <summary>
        /// Warning without exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Warning(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Warning(caller, null, message, method, lineNumber);



        /// <summary>
        /// Error with exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Error(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
        {
            if (ex != null)
                Log.Error(ex, ParceMessage(LogLevel.Error, caller, message, method, lineNumber));
            else
                Log.Error(ParceMessage(LogLevel.Error, caller, message, method, lineNumber));

            Logged?.Invoke(caller, new QLogEventArgs(LogLevel.Error, caller, ex, message, method, lineNumber));
        }

        /// <summary>
        /// Error without Exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Error(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Error(caller, null, message, method, lineNumber);



        /// <summary>
        /// Fatal with exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Fatal(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
        {
            if (ex != null)
                Log.Fatal(ex, ParceMessage(LogLevel.Fatal, caller, message, method, lineNumber));
            else
                Log.Fatal(ParceMessage(LogLevel.Fatal, caller, message, method, lineNumber));

            Logged?.Invoke(caller, new QLogEventArgs(LogLevel.Fatal, caller, ex, message, method, lineNumber));
        }
        
        /// <summary>
        /// Fatal without exception
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Fatal(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Fatal(caller, null, message, method, lineNumber);

        internal static string ParceMessage(LogLevel level, object sender, string message, string callerName, int lineNumber)
        {
            var text = level.ToString().ToUpper() + ": ";

            if (sender is Type t)
                text += t.ToString();
            else if (sender is string className)
                text += className;
            else if (sender is object)
                text += sender.GetType();

            if (!string.IsNullOrEmpty(callerName))
                text += "." + callerName;

            if (lineNumber != default && (level & QLog.AddLineNumber) > 0)
                text += $"[{lineNumber}]";

            text += " : \n\n";

            if (!string.IsNullOrEmpty(message))
                text += message + "\n\n";

            return text;
        }


    }
}