using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MonkeyBot.Modules
{
    [Name("Misc")]
    public class MiscModule : ModuleBase
    {
        [Command("ping")]
        [Summary("Pings the bot")]
        public async Task Ping()
        {
            // TODO: Add how long it took for the bot to respond "Pong!"
            await ReplyAsync("Pong!");
        }
    }
}
