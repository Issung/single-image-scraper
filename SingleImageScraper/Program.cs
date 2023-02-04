using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SingleImageScraper
{
    public class Program
    {
        static readonly CancellationTokenSource cancellationTokenSource = new();

        public static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CmdOptions>(args)
                .WithNotParsed(OptionsError)
                .WithParsedAsync(Run);
        }

        private static void OptionsError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }

        private static async Task Run(CmdOptions options)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            var outputPath = options.OutputPath 
                ?? Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) 
                ?? Path.GetDirectoryName(Environment.ProcessPath) 
                ?? string.Empty!;

            var scraper = new Scraper(options.ScrapeUrl,
                outputPath,
                options.Interval);

            await scraper.Start(cancellationTokenSource.Token);

            Console.Write("Finished. Press any key to exit.");
            Console.ReadKey();
        }

        private static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Cancelling...");
            cancellationTokenSource.Cancel();
            e.Cancel = true;
        }
    }
}
