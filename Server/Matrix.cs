using ListExt;

namespace Mtrx;

public class Matrix { 
  private List<List<double>> _matrix { get; }

  public Matrix(List<List<double>> matrix) {
    _matrix = matrix; 
  }

    public Matrix(Matrix matrix)
    {
        _matrix = matrix._matrix;
    }

    public Matrix(int rank) {
    List<List<double>> matrix = new();
    
    for (int i = 0; i < rank; ++i) {
      matrix.Add(new(rank));
    }

    _matrix = matrix;
  }

  public List<double> GetRow(int rowIndex) {
    return _matrix[rowIndex]; 
  }

  public List<double> GetColumn(int columnIndex) {
    List<double> result = new();

    for (int i = 0; i < _matrix.Count; ++i) {
      result.Add(_matrix[i][columnIndex]);
    }

    return result;
  }

  public void ShowMatrix() {
    for (int i = 0; i < _matrix.Count; ++i) { 
      for (int j = 0; j < _matrix[0].Count; ++j) {
        Console.Write($"{_matrix[i][j]} ");
      }
      Console.WriteLine();
    }
  }

  public double this[int i, int j] {
    get { return _matrix[i][j]; }
  }

  public static Matrix operator *(Matrix A, Matrix B ) {
    List<List<double>> result = new();

    for (int i = 0; i < A._matrix.Count; ++i) {
      List<double> row = new();

      for (int j = 0; j < A._matrix[0].Count; ++j) {
        row.Add(A.GetRow(i).MultiplyLists(B.GetColumn(j)));
      }

      result.Add(row);
    } 

    return new(result);
  }

  public static Matrix operator +(Matrix A, Matrix B) {
    List<List<double>> result = new();

    for (int i = 0; i < A._matrix.Count; ++i) {
      List<double> row = new();

      for (int j = 0; j < A._matrix[0].Count; ++j) {
        row.Add(A[i, j] + B[i, j]);
      }

      result.Add(row);
    }

    return new(result);
  }
}
