using ListExt;

namespace Matrices.LesSolver;

using Numerics = MathNet.Numerics.LinearAlgebra;

public class LesSolver {
  private readonly BlockMatrix _A;
  private readonly BlockMatrix _B;
  private readonly int _exp;

  private BlockMatrix _prevX;
  private BlockMatrix _X;  

  public LesSolver(BlockMatrix a, BlockMatrix b, int exp = 5) {
    _A = a;
    _B = b;
    _exp = exp;

    _X = new(b.MatrixData.Count(), 1);
    _prevX = new(b.MatrixData.Count(), 1);
  }

  public void Solve() {
    double a, b;
    System.Console.WriteLine(Math.Pow(0.1, _exp));

    do {
      _X.ShowMatrix();
      System.Console.WriteLine(new string('-', 20));

      _X = FindNewX();

      a = _prevX[0, 0][0, 0];
      b = _X[0, 0][0, 0]; 

      _prevX = _X;
    } while (Math.Abs(a - b) > Math.Pow(0.1, _exp));     
  }

  public BlockMatrix FindNewX() {
    List<Matrix> result = new();
    
    for (int i = 0; i < _B.Rows; ++i) {
      Matrix sum = new(_B.MatrixData.Count, 1);

      for (int j = 0; j < _A.Cols; ++j) {
        if (j == i) continue;
        sum += _A[i, j] * _prevX[j, 0];
      }

      var sub = _B[i, 0] - sum;
      var res = sub / _A[i, i];

      result.Add(res);
    } 

    return new(result);
  }
}
