using CommandLine;
using System;

namespace SingleImageScraper
{
    public class CmdOptions
    {
        [Option('o', "outputpath", HelpText = "Directory to output files to. Default is the application running directory.")]
        public string? OutputPath { get; set; }

        [Option('i', "inputurl", Required = true, HelpText = "URL to routinely scrape from.")]
        public string ScrapeUrl { get; set; }

        [Option('t', "timeinterval", HelpText = "Interval between scrapes, format HH:MM:SS. Default 1 minute.")]
        public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);
    }
}
