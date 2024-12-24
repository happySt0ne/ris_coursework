using ListExt;

namespace Matrices;

using Numerics = MathNet.Numerics.LinearAlgebra;

public class BlockMatrix {
  public List<List<Matrix>> MatrixData { get; }

  public int Rows { get => MatrixData.Count; }
  public int Cols { get => MatrixData[0].Count; }

  public BlockMatrix() => MatrixData = new();

  public bool IsUneven { get; private set; }

  public Matrix this[int i, int j] {
    get {
      return MatrixData[i][j];
    }
  }
  
  public BlockMatrix(int rows, int? cols = null!) {
    cols ??= rows;
    MatrixData = new();

    for (int i = 0; i < rows; ++i) {
      List<Matrix> row = new();

      for (int j = 0; j < cols; ++j) {
        row.Add(new(2, (cols == 1) ? 1 : 2));
      }

      MatrixData.Add(row);
    }
  }

  public BlockMatrix(in List<double> vector) {
    List<Matrix> matrices = new();
    MatrixData = new();

    for (int i = 0; i < vector.Count; i += 2) {
      List<List<double>> z = new();

      z.Add(new() {vector[i]});
      z.Add(new() {vector[i+1]});

      matrices.Add(new(z));
    }

    for (int i = 0; i < matrices.Count; i++ ) {
      MatrixData.Add(new() {matrices[i]});
    }
  }

  public BlockMatrix(in List<Matrix> vector) {
    MatrixData = new();
    List<Matrix> matrices = new();

    for (int i = 0; i < vector.Count; i++) {
      MatrixData.Add(new() {vector[i]});
    }
  }

  public BlockMatrix(in List<List<double>> matrix) {
    List<Matrix> matrices = new();
    MatrixData = new();

    IsUneven = (matrix.Count % 2 != 0);

    for (int i = 0; i < matrix.Count; i += 2) {
      for (int j = 0; j < matrix[0].Count; j += 2) {
        List<List<double>> z = new();

        double rightHight;
        double rightLow;

        if (IsUneven && matrix.Count - 1 == j) {
          rightHight = 0;
          rightLow = 0;
        } else {
          rightHight = matrix[i][j+1];
          try {
            rightLow = matrix[i+1][j+1];
          } catch { 
            rightLow = 0;
          }
        }

        if (IsUneven && matrix.Count - 1 == i) {
          z.Add(new List<double> { matrix[i][j], rightHight });
          z.Add(new List<double> { 0,  0 });
        } else {
          z.Add(new List<double> { matrix[i][j], rightHight });
          z.Add(new List<double> { matrix[i+1][j], rightLow });
        }

        matrices.Add(new(z));
      }
    }

    var newMatrixLenght = matrix.Count / 2;
    newMatrixLenght += IsUneven ? 1 : 0;

    for (int i = 0; i < matrices.Count; i += newMatrixLenght) {
      MatrixData.Add(matrices[i..(i + newMatrixLenght)]);
    }
  }

  public BlockMatrix (in double[,] arr, BlockMatrixOption option = BlockMatrixOption.Vector) {
    if (option is BlockMatrixOption.Matrix) {
      new BlockMatrix(arr);
      return;
    }

    IsUneven = (arr.GetLength(0) % 2) != 0;

    List<Matrix> matrices = new();
    MatrixData = new();

    for (int i = 0; i < arr.GetLength(0); i += 2) {
      List<List<double>> z = new();

      z.Add(new() {arr[i, 0]});
      
      if (IsUneven && arr.GetLength(0) - 1 == i) {
        z.Add(new() {0});
      } else {
        z.Add(new() {arr[i+1, 0]});
      }

      matrices.Add(new(z));
    }

    for (int i = 0; i < matrices.Count; i++ ) {
      MatrixData.Add(new() {matrices[i]});
    }
  }
  
  private BlockMatrix(in double[,] arr) {
    List<Matrix> matrices = new();
    MatrixData = new();

    for (int i = 0; i < arr.GetLength(0); i += 2) {
      for (int j = 0; j < arr.GetLength(1); j += 2) {
        List<List<double>> z = new();
        
        if (arr.GetLength(1) > 1) { 
          z.Add(new List<double> {arr[i, j], arr[i, j+1]});
          z.Add(new List<double> {arr[i+1, j], arr[i+1, j+1]});
        } else {
          z.Add(new List<double> {arr[i, j]});
          z.Add(new List<double> {arr[i+1, j]});
        }

        matrices.Add(new(z));
      }
    }

    var newMatrixLenght = arr.GetLength(0) / 2;
    newMatrixLenght = (newMatrixLenght == 0) ? 1 : newMatrixLenght;
    
    if (arr.GetLength(1) < 2) {
      for (int i = 0; i < matrices.Count; ++i) {
        MatrixData.Add(new() {matrices[i]});
      }
    } else {
      for (int i = 0; i < matrices.Count; i += newMatrixLenght) {
        MatrixData.Add(matrices[i..(i + newMatrixLenght)]);
      }
    }
  }

  public BlockMatrix(List<List<Matrix>> matrix) {
    MatrixData = matrix;
  }

  public void ShowMatrix() {
    for (int i = 0; i < MatrixData.Count; ++i) {
      List<double> row = new();
      List<double> row2 = new();

      for (int j = 0; j < MatrixData[0].Count; ++j) {
        row.AddRange(MatrixData[i][j].GetRow(0));
        row2.AddRange(MatrixData[i][j].GetRow(1));
      }
      
      row.ForEach(i => System.Console.Write(i + " "));
      System.Console.WriteLine();
      row2.ForEach(i => System.Console.Write(i + " "));
      System.Console.WriteLine();
    }
  }

  public List<Matrix> GetRow(int rowIndex)
    => MatrixData[rowIndex];
  
  public List<Matrix> GetColumn(int colIndex) 
    => MatrixData.Select(row => row[colIndex]).ToList();

  public static BlockMatrix operator- (BlockMatrix a,
                                       BlockMatrix b) {
    BlockMatrix result = new();
    
    for (int i = 0; i < a.MatrixData.Count; ++i) {
      List<Matrix> row = new();

      for (int j = 0; j < a.MatrixData[0].Count; ++j) {
        row.Add(a[i, j] - b[i, j]);
      }

      result.MatrixData.Add(row);
    }

    return result;
  }

  public static BlockMatrix operator* (
      BlockMatrix A, BlockMatrix B) {
    List<List<Matrix>> result = new(); 

    for (int i = 0; i < A.MatrixData.Count; ++i) {
      List<Matrix> row = new();

      for (int j = 0; j < B.MatrixData[0].Count; ++j) {
        row.Add(A.GetRow(i).MultiplyLists(B.GetColumn(j)));
      }

      result.Add(row);
    }    

    return new(result);
  }

  public static List<Matrix> operator* (
      List<Matrix> row, BlockMatrix A) {
    List<Matrix> result = new();

    for (int i = 0; i < A.MatrixData[0].Count; ++i) {
      var column = A.GetColumn(i);

      result.Add(row.MultiplyLists(column));
    }

    return result;
  }

  public override bool Equals(object? obj) {
    if (obj is null || GetType() != obj.GetType()) {
      return false;
    }
    
    BlockMatrix other = (BlockMatrix)obj;

    if (MatrixData.Count != other.MatrixData.Count) {
      return false;
    }

    var valueListA = MatrixData.SelectMany(d =>
      d.SelectMany(z => z.MatrixData.SelectMany(s => s))).ToList();

    var valueListB = other.MatrixData.SelectMany(d =>
      d.SelectMany(z => z.MatrixData.SelectMany(s => s))).ToList();

    bool result = true;

    for (int i = 0; i < valueListA.Count; ++i) {
      result &= (valueListA[i] == valueListB[i]); 
    }

    return result;
  }

  public static BlockMatrix operator+ (in BlockMatrix first, in BlockMatrix second) {
    if (first.Rows != second.Rows || first.Cols != second.Cols) {
      throw new Exception("Матрицы неодинакового размера при сложении");
    }

    List<List<Matrix>> result = new();

    for (int i = 0; i < first.Rows; ++i) {
      List<Matrix> row = new();

      for (int j = 0; j < first.Cols; ++j) {
        row.Add(first[i, j] + second[i, j]);     
      }
      result.Add(row);
    }

    return new(result);
  }

  public override int GetHashCode() => base.GetHashCode();
}
