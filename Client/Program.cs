namespace main;

using Client = Client.Client;

public class Program {
  private static async Task Main(string[] args) {
    Client.SetMatrixA(
      new(new List<List<double>>() {
        new() {2, 2, 3, 4},
        new() {4, 5, 6, 1},
        new() {2, 3, 4, 5},
        new() {5, 2, 1, 3},
      })
    );
    Client.SetMatrixB(
      new(new List<List<double>>() {
        new() {7, 8, 2, 3},
        new() {9, 3, 4, 1},
        new() {2, 3, 3, 4},
        new() {5, 1, 2, 6},
      })
    );

    await Client.Start("../Servers.json");
  }
}
