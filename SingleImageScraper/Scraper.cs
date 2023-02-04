using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SingleImageScraper
{
    public class Scraper
    {
        private readonly string url;
        private readonly string directory;
        private readonly TimeSpan scrapeInterval;
        private int index;

        public Scraper(string url, string directory, TimeSpan scrapeInterval)
        {
            this.url = url;
            this.directory = directory;
            this.scrapeInterval = scrapeInterval;

            this.index = Directory.GetFiles(directory).Length;
        }

        public async Task Start(CancellationToken token)
        {
            Console.WriteLine($"Starting scrape of {url} to directory {directory} every {scrapeInterval.TotalSeconds} seconds.\n");
            await Loop(token);
        }

        private async Task Loop(CancellationToken token)
        {
            var client = new HttpClient();

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    var result = await client.GetAsync(url, token);

                    var bytes = await result.Content.ReadAsByteArrayAsync(token);

                    Console.Write($"Downloaded image, size {bytes.Length / 1000} kilobytes... ");

                    var filename = $"Image ({index}).jpg";
                    var path = Path.Combine(directory, filename);

                    await File.WriteAllBytesAsync(path, bytes, token);

                    Console.WriteLine($"Saved to {path}.");

                    index++;
                }
                catch (OperationCanceledException oce)
                { 
                
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception occured: {e.GetType().Name}, {e.Message}");
                }

                try
                {
                    await Task.Delay(scrapeInterval, token);
                }
                catch (OperationCanceledException)
                { 
                
                }
            }
        }
    }
}
