using Client.Extensions;
using Client.Support;
using ListExt;

namespace main;

using Client = Client.Client;
using Numerics = MathNet.Numerics.LinearAlgebra;

public class Program {
  private async static Task Main(string[] args) {
    var (s, s2, path) = Input.GetPath(args);
    double[,] array = Input.GetMatrix(s);
    double[,] b = Input.GetMatrix(s2);

    Client client = new();

    Numerics.Matrix<double> numMatrix =
      Numerics.Matrix<double>.Build.DenseOfArray(array);

    var inversed = numMatrix.Inverse();
    var list = inversed.ConvertMatrixToList();

    client.SetMatrixA(list);
    client.SetMatrixB(b);

    var result = await client.Start(path);
    result.ShowMatrix();
  }
}

