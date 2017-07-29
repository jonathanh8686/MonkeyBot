using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Discord;
using Newtonsoft.Json;

namespace MonkeyBot
{
    public class Config
    {
        public static string FilePath { get; } = "config/configuration.json";

        public string BotToken { get; set; }
        public string BotPrefix { get; set; }
        public string WolframID { get; set; }
        public string GoogleGeoCodeID { get; set; }
        public string GoogleTimezoneID { get; set; }

        public static void EnsureExists()
        {
            if (!File.Exists(FilePath))
            {
                Program.Print(FilePath + " not found!", ConsoleColor.DarkRed);
                string path = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var con = new Config();

                Program.Print("Please enter BotToken: ");
                string bottoken = Console.ReadLine();

                Program.Print("Please enter Prefix: ");
                string botprefix = Console.ReadLine();

                Program.Print("Please enter Wolfram Alpha API ID: ");
                string wolframID = Console.ReadLine();

                Program.Print("Please enter GoogleMaps API ID: ");
                string googleGeoCodeID = Console.ReadLine();

                con.BotToken = bottoken;
                con.BotPrefix = botprefix;
                con.WolframID = wolframID;
                con.GoogleGeoCodeID = googleGeoCodeID;

                con.Save();
            }

            Program.Print("Configuration Loaded", ConsoleColor.Green);
        }

        public void Save()
        => File.WriteAllText(FilePath, ToJson());

        public static Config Load()
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(FilePath));
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
