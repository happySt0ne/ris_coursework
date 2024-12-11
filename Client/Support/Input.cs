using Client.Extensions;
using CommandLine;

namespace Client.Support;

public static class Input {
  public static (string, string, string) GetPath(string[] args) {
    string[] pathesToData = null!;
    string servers = null!;

    Parser.Default.ParseArguments<Options>(args)
      .WithParsed(o => {
        pathesToData = o.PathToData.ToArray();
        servers = o.PathToServersFile;
      })
      .WithNotParsed(errors => {
        foreach (var error in errors) {
          System.Console.WriteLine(error);
        }
        Environment.Exit(1);
      });
    
    return (pathesToData[0], pathesToData[1], servers);
  }

  public static double[,] GetMatrix(string pathToCsv) {
    List<List<double>> result = new();

    using (StreamReader reader = new(pathToCsv)) {
      List<double> lineValue;

      while (!reader.EndOfStream) {
        var line = reader.ReadLine();

        if (line is null) return result.CastToArray();

        lineValue = line.Trim().Split(" ").Select(double.Parse).ToList();
        result.Add(lineValue);
      }
    }

    return result.CastToArray();
  }
}
