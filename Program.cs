using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Normalize
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = ParseArgs(args);
            if (options == null)
            {
                ConsoleMessage.WriteError("Invalid arguments!");
                return;
            }
            NormalizeLineEndings(options);
        }

        private static Options ParseArgs(string[] args)
        {
            if (args == null || args.Length != 4)
            {
                return null;
            }

            var options = new Options();
            for (int i = 0; i < args.Length; i += 2)
            {
                if (args[i].ToLower() == "-p" || args[i].ToLower() == "--path")
                {
                    options.Path = args[i + 1];
                }
                else if (args[i].ToLower() == "-t" || args[i].ToLower() == "--type")
                {
                    options.LineEndingType = args[i + 1];
                }
                else
                {
                    return null;
                }
            }
            return options;
        }
        
        private static void NormalizeLineEndings(Options opts)
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
                ConsoleMessage.WriteInfo($"Normalizing file to {opts.LineEndingType.ToUpper()}...");
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
