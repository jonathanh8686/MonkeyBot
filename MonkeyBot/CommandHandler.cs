using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MonkeyBot.Modules;

namespace MonkeyBot
{
    /// <summary>
    /// Main class for handling commands etc.
    /// </summary>
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _cmds;

        private IServiceProvider _services;

        public async Task InstallCommands(DiscordSocketClient c)
        {
            _client = c;
            _cmds = new CommandService();

            _services = new ServiceCollection().BuildServiceProvider();

            await _cmds.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;

            await HandleGagged();
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;

            if (msg == null) return;
            if (msg.Author.IsBot) return;

            var context = new CommandContext(_client, msg);
            AuditLog.AddMessageEvent(s);
            StatsLog.AddMessageEvent(s);

            // Check if sender is gagged
            if (AdminModule.mutedNicknames.Contains(s.Author.Username)) { await s.DeleteAsync(); return; }

            int argPos = 0;
            if (msg.HasStringPrefix(Config.Load().BotPrefix, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "", msg.Author + ": \"" + msg.Content + "\""));
                var result = await _cmds.ExecuteAsync(context, argPos, _services);
                AuditLog.AddCommandResultEvent(result);
                if (!result.IsSuccess)
                {
                    await context.Channel.SendMessageAsync(result.ToString());
                }
            }
            else
                await Program.Log(new LogMessage(LogSeverity.Verbose, "", msg.Author + ": \"" + msg.Content + "\""));
        }

        private static async Task HandleGagged()
        {
            await Program.Log(new LogMessage(LogSeverity.Info, "CommandHandler", "Initalizing Muted Users"));
            if (File.Exists("gaggedUsers.txt"))
                AdminModule.mutedNicknames = File.ReadAllLines("gaggedUsers.txt").ToList();
            else
            {
                await Program.Log(new LogMessage(LogSeverity.Warning, "CommandHandler",
                    "No gaggedUsers.txt file found! Creating..."));
                File.Create("gaggedUsers.txt");
                await Program.Log(new LogMessage(LogSeverity.Debug, "CommandHandler", "gaggedUsers.txt file created!"));
            }
        }
    }
}
