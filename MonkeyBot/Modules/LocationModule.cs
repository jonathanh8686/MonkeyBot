using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Webhook;
using Newtonsoft.Json.Linq;

namespace MonkeyBot.Modules
{
    [Name("Location")]
    public class LocationModule : ModuleBase
    {
        [Command("coords")]
        [Summary("Gets the long/lat of a place")]
        public async Task Coords(string place)
        {
            string apiRequest = $"https://maps.googleapis.com/maps/api/geocode/json?address={place}&key={Config.Load().GoogleGeoCodeID}";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(apiRequest))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                JObject resdata = JObject.Parse(result);
                try
                {
                    string formattedAddress = (string)resdata["results"][0]["formatted_address"];

                    string lat = (string)resdata["results"][0]["geometry"]["location"]["lat"];
                    string lng = (string)resdata["results"][0]["geometry"]["location"]["lng"];
                    await ReplyAsync($"`{formattedAddress}` is located at `{lat}, {lng}`");
                }
                catch (Exception)
                {
                    await ReplyAsync("Exception: Expression could not be parsed.");
                }
            }
           
        }

        [Command("time")]
        [Summary("Gets the current time at place")]
        public async Task Time(string place)
        {
            string formattedAddress = "";
            DateTime dtAt = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            string apiRequest = $"https://maps.googleapis.com/maps/api/geocode/json?address={place}&key={Config.Load().GoogleGeoCodeID}";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(apiRequest))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                JObject resdata = JObject.Parse(result);
                try
                {
                    formattedAddress = (string)resdata["results"][0]["formatted_address"];

                    string lat = (string)resdata["results"][0]["geometry"]["location"]["lat"];
                    string lng = (string)resdata["results"][0]["geometry"]["location"]["lng"];

                    string timeRequest = $"https://maps.googleapis.com/maps/api/timezone/json?location={lat},{lng}&timestamp={DateTimeOffset.Now.ToUnixTimeSeconds()}&key={Config.Load().GoogleTimezoneID}";

                    using (HttpResponseMessage res2 = await client.GetAsync(timeRequest))
                    using (HttpContent content2 = res2.Content)
                    {
                        string result2 = await content2.ReadAsStringAsync();
                        JObject resdata2 = JObject.Parse(result2);
                        int fullOffset = (int)resdata2["dstOffset"] + (int)resdata2["rawOffset"];

                        long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        long resultUnixTime = currentUnixTime + fullOffset;

                        dtAt = dtAt.AddSeconds(resultUnixTime);
                        await ReplyAsync($"The current date/time at `{formattedAddress}` is `{dtAt}`");
                    }
                }
                catch (Exception)
                {
                    await ReplyAsync("Exception: Expression could not be parsed.");
                }
            }

        }
    }
}
