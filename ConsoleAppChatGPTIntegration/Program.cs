using Humanizer;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;

namespace ConsoleAppChatGPTIntegration
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    internal class Program
    {
        protected Program()
        {
        }

        private static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            AnsiConsole.MarkupLine("Starting commandline for [underline bold green]Chat GPT[/] World!");

            AnsiConsole.Write(new FigletText("Console GPT chat").Color(Color.NavajoWhite1));

            var config = BuildConfig();
#pragma warning disable CS8604 // Possible null reference argument.
            IOpenAIProxy chatOpenAI = new OpenAIProxy(
                apiKey: config["OpenAI:ApiKey"],
                organizationId: config["OpenAI:OrganizationId"]);
#pragma warning restore CS8604 // Possible null reference argument.

            chatOpenAI.SetSystemMessage("You are a helpful assistant called Felix AI");

            var msg = AnsiConsole.Ask<string>("[bold blue]Type your first Prompt[/]:");
            do
            {
                var results = await chatOpenAI.SendChatMessage(msg);

                foreach (var item in results)
                {
                    AnsiConsole.MarkupLineInterpolated($"[red]{item.Role.Humanize(LetterCasing.Title)}: [/] {item.Content}");
                }

                msg = AnsiConsole.Ask<string>("[bold blue]Next Prompt[/]:");
            } while (msg != "q");


            static IConfiguration BuildConfig()
            {
                var dir = Directory.GetCurrentDirectory();
                var configBuilder = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false)
                    .AddUserSecrets(Assembly.GetExecutingAssembly());

                return configBuilder.Build();
            }
        }

        private string GetDebuggerDisplay() => ToString();
    }
}