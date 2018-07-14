using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using CommandLine;

namespace Normalize
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var error in errs)
            {
                Console.WriteLine(error.Tag);
            }
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            var watch = new Stopwatch();
            watch.Start();
            var lineEnding = GetLineEnding(opts.LineEndingType);
            if (lineEnding == string.Empty)
            {
                ConsoleMessage.WriteError($"{opts.LineEndingType} is not supported!");
                return;
            }
            if (File.Exists(opts.Path))
            {
                ConsoleMessage.WriteInfo($"Normalizing file to {opts.LineEndingType} line ending...");
                NormalizeFile(opts.Path, lineEnding);
                watch.Stop();
                ConsoleMessage.WriteSuccess($"Normalization finished in {watch.Elapsed.TotalSeconds} sec.");
            }
            else if (Directory.Exists(opts.Path))
            {
                ConsoleMessage.WriteInfo($"Normalizing directory to {opts.LineEndingType} line ending...");
                NormalizeDirectory(opts.Path, lineEnding);
                watch.Stop();
                ConsoleMessage.WriteSuccess($"Normalization finished in {watch.Elapsed.TotalSeconds} sec.");
            }
            else
            {
                ConsoleMessage.WriteError($"Cannot find {opts.Path}");
            }

        }

        private static void NormalizeDirectory(string path, string lineEnding)
        {
            var files = Directory.EnumerateFiles(path);
            foreach (var file in files)
            {
                NormalizeFile(file, lineEnding);
            }
        }

        private static void NormalizeFile(string path, string lineEnding)
        {
            ConsoleMessage.WriteInfo($"Normalizing file {path}");

            var content = File.ReadAllText(path);
            var normalized = Regex.Replace(content, @"\r\n|\n\r|\n|\r", lineEnding);
            File.WriteAllText(path, normalized);
        }

        private static string GetLineEnding(string lineEndingType)
        {
            if (lineEndingType.ToUpper() == "CRLF")
            {
                return "\r\n";
            }
            else if (lineEndingType.ToUpper() == "CR")
            {
                return "\r";
            }
            else if (lineEndingType.ToUpper() == "LF")
            {
                return "\n";
            }
            else
            {
                return "";
            }

        }
    }
}
