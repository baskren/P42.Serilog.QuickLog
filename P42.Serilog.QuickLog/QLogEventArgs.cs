using Newtonsoft.Json;
using System;
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
        internal QLogEventArgs(LogLevel logLevel, object caller, Exception exception, string message, string callerMethod, int lineNumber)
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
            => ParceMessage(Level, Caller, Exception, Message, CallerMethod, CallerLineNumber);

        protected static string ParceMessage(LogLevel level, object sender, Exception ex, string message, string callerName, int lineNumber)
        {
            var text = QLog.ParceMessage(level, sender, message, callerName, lineNumber);

            if (ex != null)
            {
                if ((level & QLog.SerializeExceptionsToJson)>0)
                {
                    var json = JsonConvert.SerializeObject(ex, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });
                    text += json;
                }
                else
                    text += ExceptionMessageGenerator(ex);

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