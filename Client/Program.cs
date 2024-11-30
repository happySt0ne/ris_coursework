using ListExt;
using Matrices;

namespace main;

using Client = Client.Client;

public class Program {
  private async static Task Main(string[] args) {
    /*Client.SetMatrixA(*/
    /*  new(new List<List<double>>() {*/
    /*    new() {2, 2, 3, 4},*/
    /*    new() {4, 5, 6, 1},*/
    /*    new() {2, 3, 4, 5},*/
    /*    new() {5, 2, 1, 3},*/
    /*  })*/
    /*);*/
    /*Client.SetMatrixB(*/
    /*  new(new List<List<double>>() {*/
    /*    new() {7, 8, 2, 3},*/
    /*    new() {9, 3, 4, 1},*/
    /*    new() {2, 3, 3, 4},*/
    /*    new() {5, 1, 2, 6},*/
    /*  })*/
    /*);*/
    Client client = new();

    client.SetMatrixA(
      new List<List<double>> {
        new() {2, 2, 3, 4, 4, 5},
        new() {4, 5, 6, 1, 2, 3},
        new() {2, 3, 4, 7, 2, 8},
        new() {1, 2, 4, 4, 8, 9},
        new() {6, 8, 8, 9, 2, 1},
        new() {6, 7, 9, 0, 1, 3},
      }
    );

    client.SetMatrixB(
      new List<List<double>> {
        new() {7, 8, 2, 2, 4, 8},
        new() {9, 3, 4, 6, 7, 8},
        new() {2, 3, 3, 5, 4, 1},
        new() {1, 6, 4, 2, 3, 7},
        new() {5, 9, 1, 5, 4, 6},
        new() {4, 2, 8, 3, 4, 1},
      }
    );

    var a = await client.Start("../Servers.json");

    a.ShowMatrix();
  }
}
