using ListExt;
using Matrices;

namespace main;

using Client = Client.Client;
using Numerics = MathNet.Numerics.LinearAlgebra;

public class Program {
  private async static Task Main(string[] args) {
    Client client = new();
    var asdf = 
      new List<List<double>> {
        new() {1, -1, 3, 1},
        new() {4, -1, 5, 4},
        new() {2, -2, 4, 1},
        new() {1, -4, 5, -1},
      };

    double[,] array = asdf.convertToArr();
    Numerics.Matrix<double> numMatrix = Numerics.Matrix<double>.Build.DenseOfArray(array);
    var inversed = numMatrix.Inverse();

    var list = ListExtensions.ConvertMatrixToList(inversed);

    client.SetMatrixA(list);
    client.SetMatrixB(new List<double> {5, 4, 6, 3});

    var result = await client.Start("../Servers.json");
    result.ShowMatrix();
  }
}
