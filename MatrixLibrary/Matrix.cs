using ListExt;

using Numerics = MathNet.Numerics.LinearAlgebra;

namespace Matrices;

public class Matrix { 
  public List<List<double>> MatrixData { get; set; }

  public int Rows { get => MatrixData.Count; }
  public int Cols { get => MatrixData[0].Count; }

  public Matrix(int rows, int? cols = null) {
    cols ??= rows;
    MatrixData = new();

    for (int i = 0; i < rows; ++i) {
      List<double> row = new();

      for (int j = 0; j < cols; ++j) {
        row.Add(0);
      }

      MatrixData.Add(row);
    }
  }

  public Matrix(List<List<double>> matrix) {
    MatrixData = matrix; 
  }

  public Matrix(Matrix matrix) {
      MatrixData = matrix.MatrixData;
  }

  public Matrix() {
    MatrixData = new();
  }

  public List<double> GetRow(in int rowIndex) {
    return MatrixData[rowIndex]; 
  }

  public List<double> GetColumn(in int columnIndex) {
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

  public static Matrix operator *(in Matrix A, in Matrix B ) {
    List<List<double>> result = new();

    for (int i = 0; i < A.MatrixData.Count; ++i) {
      List<double> row = new();

      for (int j = 0; j < B.MatrixData[0].Count; ++j) {
        row.Add(A.GetRow(i).MultiplyLists(B.GetColumn(j)));
      }

      result.Add(row);
    } 

    return new(result);
  }

  public static Matrix operator -(in Matrix a, in Matrix b) {
    Matrix result = new();

    for (int i = 0; i < a.MatrixData.Count; ++i) {
      List<double> row = new();

      for (int j = 0; j < a.MatrixData[0].Count; ++j) {
        row.Add(a[i, j] - b[i, j]);
      }

      result.MatrixData.Add(row);
    }

    return result;
  }

  public static Matrix operator +(in Matrix A, in Matrix B) {
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

  public static Matrix operator/ (in Matrix first, in Matrix second) {
    var matrix = (Numerics.Matrix<double>)second;
    var inversed = matrix.Inverse();

    var myInversedMatrix = (Matrix)inversed;

    var result = first * myInversedMatrix;

    return result;
  }

  public Matrix Divide (in Matrix second) {
    if (Rows != second.Cols) throw new Exception ("no!)");
    List<List<double>> result = new();

    for (int i = 0; i < Rows; ++i) {
      List<double> row = new();

      for (int j = 0; j < Cols; ++j) {
        row.Add(MatrixData[i][j] / second[i, 0]);         
      }
      result.Add(row);
    }
    
    return new(result);
  }

  public static explicit operator Numerics.Matrix<double> (in Matrix matrix) {
    int rows = matrix.MatrixData.Count;
    int cols = matrix.MatrixData[0].Count;

    double[,] array = new double[rows, cols];

    for (int i = 0; i < rows; ++i) {
      for (int j = 0; j < cols; ++j) {
        array[i, j] = matrix[i, j];
      }
    }

    return Numerics.Matrix<double>.Build.DenseOfArray(array);
  }

  public static explicit operator Matrix (in Numerics.Matrix<double> matrix) {
    List<List<double>> result = new();

    for (int i = 0; i < matrix.RowCount; ++i) {
      List<double> row = new();

      for (int j = 0;  j < matrix.ColumnCount; ++j) {
        row.Add(matrix[i, j]);
      }
      result.Add(row);
    }

    return new(result);
  }
}
