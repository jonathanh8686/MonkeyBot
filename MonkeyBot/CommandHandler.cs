using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MonkeyBot
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _cmds;

        public async Task InstallAsync(DiscordSocketClient c)
        {
            _client = c;
            _cmds = new CommandService();
            await _cmds.AddModulesAsync(Assembly.GetEntryAssembly()); 
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null)
                return;

            var context = new SocketCommandContext(_client, msg);

            
            int argPos = 0;
            if (msg.HasStringPrefix(Config.Load().BotPrefix, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "", msg.Author + " said " + msg.Content));
                var result = await _cmds.ExecuteAsync(context, argPos);

                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ToString());
            }
            else
            {
                await Program.Log(new LogMessage(LogSeverity.Verbose, "", msg.Author + " said " + msg.Content));
                AuditLog.AddMessageEvent(s);
            }
        }
    }
}
