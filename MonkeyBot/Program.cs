using System;
using System.Threading.Tasks;
using Discord;

namespace MonkeyBot
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        private Task Log(LogMessage msg, LogSeverity logSev)
        {
            switch (msg.Severity)
            {
                    case LogSeverity.Info:
                        Print("INFO: " + msg.Message, ConsoleColor.Cyan);
                        break;
                    case LogSeverity.Verbose:
                        Print("VERBOSE: " + msg.Message, ConsoleColor.Cyan);
                        break;
                    case LogSeverity.Debug:
                        Print("DEBUG: " + msg.Message, ConsoleColor.DarkGreen);
                        break;
                    case LogSeverity.Warning:
                        Print("WARN: " + msg.Message, ConsoleColor.Yellow);
                        break;
                    case LogSeverity.Error:
                        Print("ERROR: " + msg.Message, ConsoleColor.DarkRed);
                        break;
                    case LogSeverity.Critical:
                        Print("CRITICAL: " + msg.Message, ConsoleColor.Red);
                        break;
            }

            return Task.CompletedTask;
        }

        private void Print(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}