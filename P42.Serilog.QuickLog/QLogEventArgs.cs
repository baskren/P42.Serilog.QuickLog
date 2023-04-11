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
        public LogLevel Level { get; internal set; }

        /// <summary>
        /// Method that created message
        /// </summary>
        public object CallerClass { get; private set; }

        /// <summary>
        /// Exception passed to message
        /// </summary>
        public Exception Exception { get; private set; }

        public string ExceptionDump => Exception.ExceptionMessageGenerator();

        /// <summary>
        /// Title text
        /// </summary>
        public string Title { get; private set; }

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


        protected virtual string ToStringSupplement { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="callerClass"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="callerMethod"></param>
        /// <param name="lineNumber"></param>
        public QLogEventArgs(LogLevel logLevel, Exception exception, string title, string message, string callerClass, string callerMethod, int lineNumber)
        {
            Level = logLevel;
            Exception = exception;
            Title = title;
            Message = message;
            CallerClass = callerClass;
            CallerMethod = callerMethod;
            CallerLineNumber= lineNumber;
            Title = title;
        }


        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var text = Level.ToString().ToUpper() + ": ";

            if (CallerClass is Type t)
                text += t.ToString();
            else if (CallerClass is string className)
                text += className;
            else if (CallerClass is not null)
                text += CallerClass.GetType();

            if (!string.IsNullOrEmpty(CallerMethod))
                text += "." + CallerMethod;

            if (CallerLineNumber != default && (Level & QLog.AddLineNumber) > 0)
                text += $"[{CallerLineNumber}]";

            text += " : ";

            if (!string.IsNullOrEmpty(Title))
                text += "**" + Title + "**";

            text += "\n\n";

            if (!string.IsNullOrEmpty(Message))
                text += Message + "\n\n";

            if (!string.IsNullOrEmpty(ToStringSupplement))
                text += ToStringSupplement + "\n\n";

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
                    text += Exception.ExceptionMessageGenerator();

            }

            return text;
        }



    }
}