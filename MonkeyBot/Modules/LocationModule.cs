using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MonkeyBot.Modules
{
    [Name("Location")]
    public class LocationModule : ModuleBase
    {
        [Command("coords")]
        [Summary("Gets the long/lat of a city")]
        public async Task Coords()
        {
            
           
        }
    }
}
