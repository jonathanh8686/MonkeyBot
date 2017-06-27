using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace MonkeyBot
{
    public class AuditLog
    {
        public static string FilePath { get; } = "Logs/" + DateTime.Now.Day + "." + DateTime.Now.Year + ".log";

        public static void EnsureExists()
        {
            if (!File.Exists(FilePath))
            {
                Program.Print("No AuditLog file found!", ConsoleColor.DarkRed);
                File.Create(FilePath);

                Program.Print("AuditLog file created at: " + FilePath, ConsoleColor.DarkGreen);
            }
            else
                Program.Print("AuditLog file found at: " + FilePath + "!", ConsoleColor.DarkGreen);
        }

        public static void AddMessageEvent(SocketMessage msg)
            => File.AppendAllText(FilePath, msg.Timestamp.DateTime + " " + msg.Author + ": \"" + msg.Content +"\"" + Environment.NewLine);


    }
}
