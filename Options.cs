using CommandLine;

namespace Normalize
{
    public class Options
    {
        [Option('p', "path", Required = true, HelpText = "File or folder path to normalize line endings")]
        public string Path { get; set; }

        [Option('t', "type", Required = true, HelpText = "Line ending type to use")]
        public string LineEndingType { get; set; }
    }
}
