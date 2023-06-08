using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        public static LogLevel SilentLevels { get; set;} = LogLevel.Verbose | LogLevel.Debug | LogLevel.Information;

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
            if (args.CallerClass is QLogEventArgs qLogArgs)
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
                default:
                    if (args.Exception is null)
                        global::Serilog.Log.Verbose(args.ToString());
                    else
                        global::Serilog.Log.Verbose(args.Exception, args.ToString());
                    break;
            }
            
            if ((args.Level & SilentLevels) == 0)
                Logged?.Invoke(args.CallerClass, args);
        }


        /// <summary>
        /// Verbose - with Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Verbose(Exception ex, string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Verbose, ex, title, message, callerClass, method, lineNumber));

        /// <summary>
        /// Verbose - without Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Verbose(string message, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Verbose(null, message, title, callerClass, method, lineNumber);



        /// <summary>
        /// Debug - with Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Debug(Exception ex, string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Debug, ex, title, message, callerClass, method, lineNumber));

        /// <summary>
        /// Debug - without Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Debug( string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Debug(null, message, title, callerClass, method, lineNumber);



        /// <summary>
        /// Information - with Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Information(Exception ex,  string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Information, ex, title, message, callerClass, method, lineNumber));

        /// <summary>
        /// Information - without Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Information( string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Information(null, message, title, callerClass, method, lineNumber);




        /// <summary>
        /// Warning - with Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Warning(Exception ex,  string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Warning, ex, title, message, callerClass, method, lineNumber));

        /// <summary>
        /// Warning - without Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Warning( string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Warning(null, message, title, callerClass, method, lineNumber);




        /// <summary>
        /// Error - with Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Error(Exception ex,  string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Error, ex, title, message, callerClass ?? NameOfCallingClass(), method, lineNumber));

        /// <summary>
        /// Error - without Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Error( string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Error(null, message, title, callerClass ?? NameOfCallingClass(), method, lineNumber);

        /// <summary>
        /// Not Implemented - with exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void NotImplemented(Exception ex, string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.NotImplemented, ex, title, message, callerClass ?? NameOfCallingClass(), method, lineNumber));

        /// <summary>
        /// Not Implemented - without exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void NotImplemented(string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => NotImplemented(null, message, title, callerClass ?? NameOfCallingClass(), method, lineNumber);

        /// <summary>
        /// Fatal - with Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Fatal(Exception ex,  string message = default, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Log(new QLogEventArgs(LogLevel.Fatal, ex, title, message, callerClass ?? NameOfCallingClass(), method, lineNumber));

        /// <summary>
        /// Fatal - without Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="callerClass"></param>
        /// <param name="method"></param>
        /// <param name="lineNumber"></param>
        public static void Fatal(string message, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => Fatal(null, message, title, callerClass ?? NameOfCallingClass(), method, lineNumber);

        public static async Task<PermissionState> RequestPermission(PermissionLogger args)
        {
            Log(args);
            await args.CompletedAsync();
            return args.State;
        }

        public static async Task<PermissionState> RequestPermission(string message, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => await RequestPermission(new PermissionLogger(title, message, callerClass, method, lineNumber));
        
        public static ProgressLogger ShowProgress(ProgressLogger args)
        {
            Log(args);
            return args;
        }

        public static ProgressLogger ShowProgress(string message, string title = default, string callerClass = default, [CallerMemberName] string method = default, [CallerLineNumber] int lineNumber = default)
            => ShowProgress(new ProgressLogger(title, message, callerClass ?? NameOfCallingClass(), method, lineNumber));
        


        public static string NameOfCallingClass()
        {
            string fullName;
            Type declaringType;
            int skipFrames = 2;
            do
            {
                var sf0 = new StackFrame(0, false);
                var m0 = sf0.GetMethod();
                var sf1 = new StackFrame(1, false);
                var m1 = sf1.GetMethod();
                var sf2 = new StackFrame(2, false);
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName;
        }

    }
}