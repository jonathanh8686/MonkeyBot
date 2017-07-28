using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Webhook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonkeyBot.Modules
{
    [Name("Math")]
    public class MathModule : ModuleBase
    {
        [Command("eval")]
        [Summary("Evaluates an expression (Experimental)")]
        public async Task Eval(string question)
        {
            question = question.Replace(" ", "");
            string wolframKey = Config.Load().WolframID;
            string pageURL = $"http://api.wolframalpha.com/v2/query?input={question}&appid={Config.Load().WolframID}&output=json&format=plaintext&includepodid=result";

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(pageURL))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                JObject resdata = JObject.Parse(result);
                try
                {
                    await ReplyAsync((string) resdata["queryresult"]["pods"][0]["subpods"][0]["plaintext"]);
                }
                catch (Exception)
                {
                    await ReplyAsync("Exception: Expression could not be parsed.");
                }
            }
        }

        [Command("add")]
        [Summary("Gets the sum of two numbers")]
        public async Task Add(long a, long b)
        {
            long sum = 0;
            checked
            {
                try { sum = a + b; }
                catch (OverflowException)
                {
                    await ReplyAsync("Integer Over/Under flow Detected! Numbers too large for 64-bit integers.");
                    await Program.Log(new LogMessage(LogSeverity.Error, "MathModule", "Integer Overflow Detected! Numbers too large for 64-bit integers."));

                    return;
                }
            }

            await Program.Log(new LogMessage(LogSeverity.Info, "MathModule", $"{a} + {b} = {sum}"));
            await ReplyAsync($"{a} + {b} = {sum}");
        }

        [Command("subtract")]
        [Summary("Gets the difference of two numbers")]
        public async Task Subtract(long a, long b)
        {
            long diff = 0;
            checked
            {
                try { diff = a - b; }
                catch (OverflowException)
                {
                    await ReplyAsync("Integer Over/Under flow Detected! Numbers too large for 64-bit integers.");
                    await Program.Log(new LogMessage(LogSeverity.Error, "MathModule", "Integer Overflow Detected! Numbers too large for 64-bit integers."));

                    return;
                }
            }

            await Program.Log(new LogMessage(LogSeverity.Info, "MathModule", $"{a} - {b} = {diff}"));
            await ReplyAsync($"{a} - {b} = {diff}");
        }

        [Command("multiply")]
        [Summary("Get the product of two numbers")]
        public async Task Multiply(long a, long b)
        {
            long product = 0;
            checked
            {
                try { product = a * b; }
                catch (OverflowException)
                {
                    await ReplyAsync("Integer Over/Under flow Detected! Numbers too large for 64-bit integers.");
                    await Program.Log(new LogMessage(LogSeverity.Error, "MathModule", "Integer Overflow Detected! Numbers too large for 64-bit integers."));

                    return;
                }
            }

            await Program.Log(new LogMessage(LogSeverity.Info, "MathModule", $"{a} * {b} = {product}"));
            await ReplyAsync($"{a} * {b} = {product}");
        }

        [Command("divide")]
        [Summary("Get the quotient of two numbers")]
        public async Task Divide(float a, float b)
        {
            if (b == 0)
            {
                await ReplyAsync("Divide by 0 Error!");
                return;
            }
            float quotient = 0;
            checked
            {
                try { quotient = a / b; }
                catch (OverflowException)
                {
                    await ReplyAsync("Integer Over/Under flow Detected! Numbers too large for 64-bit integers.");
                    await Program.Log(new LogMessage(LogSeverity.Error, "MathModule", "Integer Overflow Detected! Numbers too large for 64-bit integers."));

                    return;
                }
            }

            await Program.Log(new LogMessage(LogSeverity.Info, "MathModule", $"{a} / {b} = {quotient}"));
            await ReplyAsync($"{a} / {b} = {quotient}");
        }

        [Command("square")]
        [Summary("Squares a number")]
        public async Task Square(long a)
        => await Power(a, 2);

        [Command("power")]
        [Summary("Raises a number to a power")]
        public async Task Power(long a, ulong b)
        {
            long ans = 0;
            checked
            {
                try
                {
                    ans = (long)Math.Pow(a, b); 
                }
                catch (OverflowException)
                {
                    await ReplyAsync("Integer Overflow Detected! Numbers too large for 64-bit integers.");
                    await Program.Log(new LogMessage(LogSeverity.Error, "MathModule",
                        "Integer Overflow Detected! Numbers too large for 64-bit integers."));

                    return;
                }
            }

            await Program.Log(new LogMessage(LogSeverity.Info, "MathModule", $"{a} ^ {b} = {ans}"));
            await ReplyAsync($"{a} ^ {b} = {ans}");
        }
    }
}
