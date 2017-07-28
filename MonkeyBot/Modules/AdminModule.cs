using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MonkeyBot.Modules
{
    [Name("Admin")]
    [Group("admin")]
    public class AdminModule : ModuleBase
    {
        public static List<string> mutedNicknames = new List<string>();

        [Command("gag")]
        [Summary("Instantly removes messages from a user")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task Gag(string username)
        {
            if (mutedNicknames.Contains(username))
                await ReplyAsync($"{username} is already gagged!");

            mutedNicknames.Add(username);
            File.WriteAllLines("gaggedUsers.txt", mutedNicknames);
            await ReplyAsync($"Messages from {username} will now be deleted.");
        }

        [Command("ungag")]
        [Summary("Removes user from gag list")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task Ungag(string username)
        {
            mutedNicknames.Remove(username);
            File.WriteAllLines("gaggedUsers.txt", mutedNicknames);
            await ReplyAsync($"Messages from {username} will no longer be gagged.");
        }

        [Command("purge", RunMode = RunMode.Async)]
        [Summary("Deletes the specified amount of messages.")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeChat(int amount, string username = "")
        {
            if (username == "")
            {
                var messages = await this.Context.Channel.GetMessagesAsync(amount + 1).Flatten();
                await this.Context.Channel.DeleteMessagesAsync(messages);
            }
            else
            {

                List<IMessage> deleteMessages = new List<IMessage>();

                var messages = await Context.Channel.GetMessagesAsync(amount + 1).Flatten();
                foreach (var message in messages)
                {
                    if (message.Author.Username == username)
                        deleteMessages.Add(message);
                }
                await Context.Channel.DeleteMessagesAsync(deleteMessages);
            }
        }
    }
}
