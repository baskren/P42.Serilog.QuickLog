
namespace P42.Serilog.QuickLog
{
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// Does nothing: not sent to Serilog, QLog.Logged not fired
        /// </summary>
        None = 0,
        Fatal = 1,
        Error = 2,
        Warning = 4,
        Information = 8,
        Debug = 16,
        Verbose = 32
    }
}