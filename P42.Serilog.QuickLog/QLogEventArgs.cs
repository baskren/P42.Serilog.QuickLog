using Newtonsoft.Json;
using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace P42.Serilog.QuickLog
{
    public class QLogEventArgs : EventArgs
    {
        /// <summary>
        /// Level of Message
        /// </summary>
        public LogLevel Level { get; private set; }

        /// <summary>
        /// Method that created message
        /// </summary>
        public object Caller { get; private set; }

        /// <summary>
        /// Exception passed to message
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Message text
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Method that created message
        /// </summary>
        public string CallerMethod { get; private set; }

        /// <summary>
        /// Line number in file that created message
        /// </summary>
        public int CallerLineNumber { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="caller"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="callerMethod"></param>
        /// <param name="lineNumber"></param>
        public QLogEventArgs(LogLevel logLevel, object caller, Exception exception, string message, string callerMethod, int lineNumber)
        {
            Level = logLevel;
            Caller = caller;
            Exception = exception;
            Message = message;
            CallerMethod = callerMethod;
            CallerLineNumber= lineNumber;
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var text = Level.ToString().ToUpper() + ": ";

            if (Caller is Type t)
                text += t.ToString();
            else if (Caller is string className)
                text += className;
            else if (Caller is object)
                text += Caller.GetType();

            if (!string.IsNullOrEmpty(CallerMethod))
                text += "." + CallerMethod;

            if (CallerLineNumber != default && (Level & QLog.AddLineNumber) > 0)
                text += $"[{CallerLineNumber}]";

            text += " : \n\n";

            if (!string.IsNullOrEmpty(Message))
                text += Message + "\n\n";

            if (Exception != null)
            {
                if ((Level & QLog.SerializeExceptionsToJson) > 0)
                {
                    var json = JsonConvert.SerializeObject(Exception, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });
                    text += json;
                }
                else
                    text += ExceptionMessageGenerator(Exception);

            }

            return text;
        }


        static string ExceptionMessageGenerator(Exception e, int depth = 0)
        {
            if (e is null)
                return string.Empty;

            var prefix = string.Empty;
            for (int i = 0; i < depth; i++)
                prefix = prefix + "INNER ";

            var result = $"{prefix}EXCEPTION: {e.GetType()} {e.Message} : \n{(!string.IsNullOrWhiteSpace(e.HelpLink)?e.HelpLink+"\n":null)}{e.StackTrace}\n";
            return result + ExceptionMessageGenerator(e.InnerException, depth + 1);
        }

    }
}