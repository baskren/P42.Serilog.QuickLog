# P42.Serilog.QuickLog

Two goals:
- Make it easy to log very common events by having event pre-populated with the following information:
  - Caller class (required parameter)
  - Caller method name (automatically generated)
  - Exception Message and Inner Messages, with StackTrace (optional parameter)
  - Message (optional parameter)
  - Caller line number (optional and automatic)

- Fire events just for these quick logged events that can be subscribed to by your code for handling.

## Usage

### MyClass.cs

```cSharp
using P42.Serilog.QuickLog;

namespace MyApp
{
    public class MyClass
    {

        public void MyMethod()
        {
            try
            {
                throw new InvalidTimeZoneException("Bonkers!");
            }
            catch (Exception ex)
            {
                QLog.Error(this, ex);
            }

        }
    }
}
```

### Program.cs

```csharp
using P42.Serilog.QuickLog;
using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            QLog.Logged += OnLogged;

            var x = new MyClass();
            x.MyMethod();

        }

        private static void OnLogged(object? sender, QLogEventArgs e)
        {
            Console.WriteLine(e);
        }

    }
}
```


Resulting log message:
```
ERROR: MyApp.MyClass.MyMethod[16] :

EXCEPTION: System.InvalidTimeZoneException Bonkers! :
   at MyApp.MyClass.MyMethod() in C:\Users\ben\Development\SeriLogExtensions\P42.Serilog.QuickLog.Demo\MyClass.cs:line 12
```

## Options

### Serialize exceptions to JSON

Set which log levels will serialize exceptions as JSON (instead of pretty print) by setting `P42.Serilog.QuickLog.QLog.SerializeExceptionsToJson` to the `P42.Serilog.QuickLog.LogLevel`(s) desired.  Example:
```csharp
using P42.Serilog.QuickLog;
...
QLog.SerializeExceptionsToJson = LogLevel.Fatal | LogLevel.Error;
```

### Add Line Number

Sometime iOS and Android release builds don't do the best job capturing line numbers.  This doesn't fix that but at least it allows you to add line numbers. Set `P42.Serilog.QuickLog.AddLineNumber` to the `P42.Serilog.QuickLog.LogLevel`(s) desired.  Example:
```csharp
using P42.Serilog.QuickLog;
...
QLog.AddLineNumber = LogLevel.Fatal | LogLevel.Error;
```

### Silent Logging

To control which `LogLevels` (Verbose, Debug, Information, Warning, Error, Fatal) will **NOT** be passed along to the `QLog.Logged` event.  By default this is `LogLevels.Verbose | LogLevels.Debug`.