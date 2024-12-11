using CommandLine;

namespace Servers.Program;

internal class Options {
  [Option('p', "path",
    HelpText = "Path to file with servers addresses")]
  public string PathToFile { get; init; } = null!; 
}
