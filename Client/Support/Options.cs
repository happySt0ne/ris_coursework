using CommandLine;

namespace Client.Support;

internal class Options {
  [Option(
    'p', "path",
    Required = true,
    HelpText = "Path to files with data." 
      + "Fisrt must be path to matrix. "
      + "Second must be path to column of constants. "
  )]
  public IEnumerable<string> PathToData { get; init; } = null!;

  [Option('s', "servers", 
    HelpText = "Path to file with servers addresses"
  )]
  public string PathToServersFile { get; init; } = "../Servers.json";
}
