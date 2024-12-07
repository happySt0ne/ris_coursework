using Servers;
using CommandLine;

internal class Options {
  [Option('p', "path",
    HelpText = "Path to file with servers addresses")]
  public string PathToFile { get; init; } = null!; 
}

internal class Program {
  private static void Main(string[] args) {
    string path = string.Empty;

    var a = Parser.Default.ParseArguments<Options>(args)
      .WithParsed(o => {
          path = o.PathToFile;
      });
    
    Server server = (path is not null) ? new(path) : new();

    server.Dispose();
  } 
}
