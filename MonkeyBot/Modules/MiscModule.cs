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

        [Command("71")]
        [Summary("lmao")]
        public async Task YongjaeDmg()
        {
            await ReplyAsync("https://thumb.gyazo.com/thumb/1200/_7ce8bd0fa6f7407c0a04ed4a7a3acc57-png.jpg");
        }
    }
}
