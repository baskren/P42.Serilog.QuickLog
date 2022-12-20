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
        /// Flagged levels that will not fire QLog.Logged event
        /// </summary>
        public static LogLevel SilentLevels { get; set;} = LogLevel.Verbose | LogLevel.Debug;

        /// <summary>
        /// Event called when QLog is created
        /// </summary>
        public static event EventHandler<QLogEventArgs> Logged;

        /// <summary>
        /// Log entry
        /// </summary>
        /// <param name="args">QLogEventArgs</param>
        /// <param name="silent">Don't fire event?</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Log(QLogEventArgs args)
        {
            if (args.Caller is QLogEventArgs qLogArgs)
                args = qLogArgs;

            switch (args.Level)
            {
                case LogLevel.None:
                    return;
                case LogLevel.Fatal:
                    if (args.Exception is null)
                        global::Serilog.Log.Fatal(args.ToString());
                    else
                        global::Serilog.Log.Fatal(args.Exception, args.ToString());
                    break;
                case LogLevel.Error:
                    if (args.Exception is null)
                        global::Serilog.Log.Error(args.ToString());
                    else
                        global::Serilog.Log.Error(args.Exception, args.ToString());
                    break;
                case LogLevel.Warning:
                    if (args.Exception is null)
                        global::Serilog.Log.Warning(args.ToString());
                    else
                        global::Serilog.Log.Warning(args.Exception, args.ToString());
                    break;
                case LogLevel.Information:
                    if (args.Exception is null)
                        global::Serilog.Log.Information(args.ToString());
                    else
                        global::Serilog.Log.Information(args.Exception, args.ToString());
                    break;
                case LogLevel.Debug:
                    if (args.Exception is null)
                        global::Serilog.Log.Debug(args.ToString());
                    else
                        global::Serilog.Log.Debug(args.Exception, args.ToString());
                    break;
                case LogLevel.Verbose:
                    if (args.Exception is null)
                        global::Serilog.Log.Verbose(args.ToString());
                    else
                        global::Serilog.Log.Verbose(args.Exception, args.ToString());
                    break;
                default:
                    throw new ArgumentException($"Invalid LogLevel [{args.Level}]");
            }
            
            if ((args.Level & SilentLevels) == 0)
                Logged?.Invoke(args.Caller, args);
        }


        /// <summary>
        /// Verbose - with Exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="ex">Exception</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Verbose(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Verbose, caller, ex, message, method, lineNumber));

        /// <summary>
        /// Verbose without exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Verbose(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Verbose(caller, null, message, method, lineNumber);



        /// <summary>
        /// Debug with exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="ex">Exception</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Debug(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Debug, caller, ex, message, method, lineNumber));

        /// <summary>
        /// Debug without exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Debug(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Debug(caller, null, message, method, lineNumber);



        /// <summary>
        /// Information with exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="ex">Exception</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Information(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Information, caller, ex, message, method, lineNumber));

        /// <summary>
        /// Information without exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Information(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Information(caller, null, message, method, lineNumber);




        /// <summary>
        /// Warning with exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="ex">Exception</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Warning(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Warning, caller, ex, message, method, lineNumber));

        /// <summary>
        /// Warning without exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Warning(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Warning(caller, null, message, method, lineNumber);




        /// <summary>
        /// Error with exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="ex">Exception</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Error(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Error, caller, ex, message, method, lineNumber));

        /// <summary>
        /// Error without Exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Error(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Error(caller, null, message, method, lineNumber);



        /// <summary>
        /// Fatal with exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="ex">Exception</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Fatal(object caller, Exception ex, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Fatal, caller, ex, message, method, lineNumber));

        /// <summary>
        /// Fatal without exception
        /// </summary>
        /// <param name="caller">Caller method (string, instance, or type)</param>
        /// <param name="message">Human readable message</param>
        /// <param name="method">method</param>
        /// <param name="lineNumber">line number</param>
        public static void Fatal(object caller, string message = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Fatal(caller, null, message, method, lineNumber);


    }
}