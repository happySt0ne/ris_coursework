using ListExt;

namespace Matrices;

public class Matrix { 
  public List<List<double>> MatrixData { get; set; }

  public Matrix(List<List<double>> matrix) {
    MatrixData = matrix; 
  }

  public Matrix(Matrix matrix) {
      MatrixData = matrix.MatrixData;
  }

  public Matrix() {
    MatrixData = new();
  }

  public Matrix(int rank) {
    List<List<double>> matrix = new();
    
    for (int i = 0; i < rank; ++i) {
      matrix.Add(new(rank));
    }

    MatrixData = matrix;
  }

  public List<double> GetRow(int rowIndex) {
    return MatrixData[rowIndex]; 
  }

  public List<double> GetColumn(int columnIndex) {
    List<double> result = new();

    for (int i = 0; i < MatrixData.Count; ++i) {
      result.Add(MatrixData[i][columnIndex]);
    }

    return result;
  }

  public void ShowMatrix() {
    for (int i = 0; i < MatrixData.Count; ++i) { 
      for (int j = 0; j < MatrixData[0].Count; ++j) {
        Console.Write($"{MatrixData[i][j]} ");
      }
      Console.WriteLine();
    }
  }

  public double this[int i, int j] {
    get { return MatrixData[i][j]; }
  }

  public static Matrix operator *(Matrix A, Matrix B ) {
    List<List<double>> result = new();

    System.Console.WriteLine("a:");
    A.ShowMatrix();

    System.Console.WriteLine("b:");
    B.ShowMatrix();

    for (int i = 0; i < A.MatrixData.Count; ++i) {
      List<double> row = new();

      for (int j = 0; j < B.MatrixData[0].Count; ++j) {
        row.Add(A.GetRow(i).MultiplyLists(B.GetColumn(j)));
      }

      result.Add(row);
    } 

    return new(result);
  }

  public static Matrix operator +(Matrix A, Matrix B) {
    List<List<double>> result = new();

    for (int i = 0; i < A.MatrixData.Count; ++i) {
      List<double> row = new();

      for (int j = 0; j < A.MatrixData[0].Count; ++j) {
        row.Add(A[i, j] + B[i, j]);
      }

      result.Add(row);
    }

    return new(result);
  }

  public override int GetHashCode() => base.GetHashCode();

  public override bool Equals(object? obj) {
    if (obj is null) return false;
    if (obj is not Matrix) return false;

    var mtr = obj as Matrix;
    if (mtr is null) return false;

    if (mtr.MatrixData.Count != MatrixData.Count) return false;
    
    List<double> flatMtr = mtr.MatrixData
      .SelectMany(list => list).ToList();
    List<double> flat = MatrixData
      .SelectMany(list => list).ToList();

    for (int i = 0; i < flat.Count; ++i) {
      if (flatMtr[i] != flat[i]) return false;
    } 

    return true;
  }
}
