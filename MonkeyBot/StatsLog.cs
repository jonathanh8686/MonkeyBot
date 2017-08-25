using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace MonkeyBot
{
    public class StatsLog
    {
        private DiscordSocketClient _client;
        public static string FilePath { get; } = "Stats/" + DateTime.Now.ToString("M.d.yy") + "/";

        public static string[] FilePaths =
        {
            FilePath + "Message.log",
            FilePath + "VCJoinLeave.log",
            FilePath + "VCMutedDeafen.log",
            FilePath + "VCSelf.log"
        };

        public void Mount(DiscordSocketClient c)
        {
            _client = c;
            _client.UserVoiceStateUpdated += UserVoiceStateUpdated;
        }

        private static Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState prevState, SocketVoiceState afterState)
        {
            AddJoinLeaveVCEvent(user, prevState, afterState);
            AddMutedDeafenVCEvent(user, prevState, afterState);
            AddSelfMutedDeafenVCEvent(user, prevState, afterState);

            return Task.CompletedTask;
        }

        // ReSharper disable once InconsistentNaming
        private static void AddSelfMutedDeafenVCEvent(SocketUser user, SocketVoiceState prevState, SocketVoiceState afterState)
        {
            if (prevState.IsSelfMuted && !afterState.IsSelfMuted)
            {
                // User was unmuted
                File.AppendAllText(FilePaths[3],
                    $"{user.Username + "\r\n" + "muted" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            }
            else if (afterState.IsSelfMuted && !prevState.IsSelfMuted)
            {
                // User got muted
                File.AppendAllText(FilePaths[3],
                    $"{user.Username + "\r\n" + "umuted" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            }

            if (prevState.IsSelfDeafened && !afterState.IsSelfDeafened)
            {
                // User was unmuted
                File.AppendAllText(FilePaths[3],
                    $"{user.Username + "\r\n" + "deaf" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            }
            else if (afterState.IsSelfDeafened && !prevState.IsSelfDeafened)
            {
                // User got muted
                File.AppendAllText(FilePaths[3],
                    $"{user.Username + "\r\n" + "undeafen" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            }
        }

        // ReSharper disable once InconsistentNaming
        private static void AddMutedDeafenVCEvent(SocketUser user, SocketVoiceState prevState, SocketVoiceState afterState)
        {
            if (prevState.IsMuted && !afterState.IsMuted)
            {
                // User was unmuted
                File.AppendAllText(FilePaths[2],
                    $"{user.Username + "\r\n" + "muted" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            }
            else if (afterState.IsMuted && !prevState.IsMuted)
            {
                // User got muted
                File.AppendAllText(FilePaths[2],
                    $"{user.Username + "\r\n" + "umuted" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            }

            if (prevState.IsDeafened && !afterState.IsDeafened)
            {
                // User was unmuted
                File.AppendAllText(FilePaths[2],
                    $"{user.Username + "\r\n" + "deaf" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            }
            else if (afterState.IsDeafened && !prevState.IsDeafened)
            {
                // User got muted
                File.AppendAllText(FilePaths[2],
                    $"{user.Username + "\r\n" + "undeafen" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            }
        }

        // ReSharper disable once InconsistentNaming
        private static void AddJoinLeaveVCEvent(SocketUser user, SocketVoiceState prevState, SocketVoiceState afterState)
        {
            // Voice channels save the data in 3 lines
            // Username + VoiceChannelName + Time
            if (afterState.VoiceChannel == null)
                File.AppendAllText(FilePaths[1],
                    $"{user.Username + "\r\n" + "left" + "\r\n" + prevState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
            else if (prevState.VoiceChannel == null)
                File.AppendAllText(FilePaths[1],
                    $"{user.Username + "\r\n" + "joined" + "\r\n" + afterState.VoiceChannel.Name + "\r\n" + DateTime.Now.ToString("H:mm:ss") + "\r\n"}");
        }


        public static void EnsureExists()
        {
            if (Directory.Exists(FilePath))
                Program.Log(new LogMessage(LogSeverity.Info, "", "StatsLog Directory found!"));
            else
            {
                Program.Log(new LogMessage(LogSeverity.Error, "", "No StatsLog Directed Found"));
                Program.Log(new LogMessage(LogSeverity.Error, "", "Creating one at " + FilePath));
                Directory.CreateDirectory(FilePath);
            }

            foreach (var path in FilePaths)
            {
                if (!File.Exists(path))
                {
                    Program.Print("No Statslog file found!", ConsoleColor.DarkRed);
                    File.Create(path);

                    Program.Print("Statslog file created at: " + path, ConsoleColor.DarkGreen);
                }
                else
                    Program.Print("Statslog file found at: " + path + "!", ConsoleColor.DarkGreen);
            }
        }

        public static void AddMessageEvent(SocketMessage msg)
            // Data is saved in 3 lines
            // Message Author, Message Content, Time
            => File.AppendAllText(FilePaths[0], $"{msg.Author + "\r\n" + msg.Content + "\r\n" + msg.CreatedAt.DateTime.ToString("H:mm:ss") + "\r\n"}");
    }


}

