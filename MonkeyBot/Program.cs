using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Newtonsoft.Json;

namespace MonkeyBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _cmdHandler;
        private AuditLog _auditLog;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            await Log(new LogMessage(LogSeverity.Verbose, "", "Bot Started..."));
            Console.ForegroundColor = ConsoleColor.White;

            Config.EnsureExists();
            AuditLog.EnsureExists();

            var botdata = Config.Load();
            
            // Check if the .json file contains "BotPrefix"
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 2000,
            });

            _client.Log += Log;


            _auditLog = new AuditLog();
             _auditLog.Mount(_client);

            _cmdHandler = new CommandHandler();
            await _cmdHandler.InstallAsync(_client);

            await Log(new LogMessage(LogSeverity.Info, "", "BotToken: " + botdata.BotToken + " located!"));
            Console.WriteLine("");

            await Login(botdata);
            await Task.Delay(-1);
        }

        /// <summary>
        /// Login and Connect to the Discord
        /// </summary>
        /// <param name="botdata">Config class containing data about the bot</param>
        private async Task Login(Config botdata)
        {
            try
            {
                await _client.LoginAsync(TokenType.Bot, botdata.BotToken);
                await _client.StartAsync();
            }
            catch (Exception ex)
            {
                // Will throw "401: Unauthorized" if the BotToken is invalid
                await Log(new LogMessage(LogSeverity.Critical, "", ex.Message));
                await Log(new LogMessage(LogSeverity.Error, "", "Possibly invalid token?"));
            }
        }

        /// <summary>
        /// Logs data to the Console
        /// </summary>
        /// <param name="msg">LogMessage object to describe log data</param>
        public static Task Log(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Info:
                    Print("INFO: " + msg.Message, ConsoleColor.Cyan);
                    break;
                case LogSeverity.Verbose:
                    Print("VERBOSE: " + msg.Message, ConsoleColor.Green);
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

        /// <summary>
        /// Simple print data to Console
        /// </summary>
        /// <param name="msg">String to print to the Console</param>
        /// <param name="color">Color of print</param>
        public static void Print(string msg, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now + "\t" + msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
