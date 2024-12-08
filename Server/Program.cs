using CommandLine;

namespace Servers.Program;

internal class Program {
  private static bool _isRunning = true;

  private static void Main(string[] args) {
    string path = GetPath(args);
    Server server = (path is not null) ? new(path) : new();

    Thread input = new(Input);
    input.Start();

    server.Start();

    Console.CancelKeyPress += (_, _) => server.Dispose();

    while (_isRunning);

    input.Join();
    server.Dispose();
  } 

  private static string GetPath(string[] args) {
    string result = string.Empty;

    Parser.Default.ParseArguments<Options>(args)
      .WithParsed(o => result = o.PathToFile);

    return result;
  }

  private static void Input() {
    Console.WriteLine("Press 'q' to exit.");

    while (_isRunning) {
      ConsoleKeyInfo keyInfo = Console.ReadKey(true);

      if (keyInfo.Key == ConsoleKey.Q) {
        _isRunning = false;
      }
    }
  }
}
