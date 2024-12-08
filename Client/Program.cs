using ListExt;
using Matrices;

namespace main;

using Client = Client.Client;

public class Program {
  private async static Task Main(string[] args) {
    Client client = new();

    client.SetMatrixA(
      new List<List<double>> {
        new() {1, -1, 3, 1},
        new() {4, -1, 5, 4},
        new() {2, -2, 4, 1},
        new() {1, -4, 5, -1},
      }
    );

    List<double> ass = new() {5, 4, 6, 3};
    BlockMatrix x = new(ass);

    client.SetMatrixB(ass);

    var a = await client.Start("../Servers.json");
    
    System.Console.WriteLine("multiplying result");
    a.ShowMatrix();
  }
}
