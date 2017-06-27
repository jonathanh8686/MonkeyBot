using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace MonkeyBot
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            await Log(new LogMessage(LogSeverity.Verbose, "", "Bot Started..."));
            Console.ForegroundColor = ConsoleColor.White;

            Config.EnsureExists();
            Config botdata = Config.Load();

            // Check if the .json file contains "BotPrefix"
            var client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 1000,
            });


            client.Log += Log;

            await Log(new LogMessage(LogSeverity.Info, "", "BotToken: " + botdata.BotToken + " located!"));
            Console.WriteLine("");

            try
            {
                // Login to Dicords - Connects Bot
                await client.LoginAsync(TokenType.Bot, botdata.BotToken);
                await client.StartAsync();
            }
            catch (Exception ex)
            {
                // Will throw "401: Unauthorized" if the BotToken is invalid
                await Log(new LogMessage(LogSeverity.Critical, "", ex.Message));
                await Log(new LogMessage(LogSeverity.Error, "", "Possibly invalid token?"));
            }

            await Task.Delay(-1);
        }
        /// <summary>
        /// Logs data to the Console
        /// </summary>
        /// <param name="msg">LogMessage object to describe log data</param>
        private static Task Log(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Info:
                    Print(DateTime.Now + "\t" + "INFO: " + msg.Message, ConsoleColor.Cyan);
                    break;
                case LogSeverity.Verbose:
                    Print(DateTime.Now + "\t" + "VERBOSE: " + msg.Message, ConsoleColor.Cyan);
                    break;
                case LogSeverity.Debug:
                    Print(DateTime.Now + "\t" + "DEBUG: " + msg.Message, ConsoleColor.DarkGreen);
                    break;
                case LogSeverity.Warning:
                    Print(DateTime.Now + "\t" + "WARN: " + msg.Message, ConsoleColor.Yellow);
                    break;
                case LogSeverity.Error:
                    Print(DateTime.Now + "\t" + "ERROR: " + msg.Message, ConsoleColor.DarkRed);
                    break;
                case LogSeverity.Critical:
                    Print(DateTime.Now + "\t" + "CRITICAL: " + msg.Message, ConsoleColor.Red);
                    break;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Simple print data to Console
        /// </summary>
        /// <param name="msg">String to print to the Console</param>
        /// <param name="color">Color of print</param>
        public static void Print(string msg, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
