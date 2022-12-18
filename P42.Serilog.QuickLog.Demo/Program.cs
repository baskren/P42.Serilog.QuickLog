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