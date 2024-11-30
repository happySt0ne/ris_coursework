using ListExt;

namespace Matrices;

public class BlockMatrix {
  public List<List<Matrix>> MatrixData { get; }

  public BlockMatrix() => MatrixData = new();

  public BlockMatrix(List<List<double>> matrix) {
    List<Matrix> matrices = new();
    MatrixData = new();

    for (int i = 0; i < matrix.Count; i += 2) {
      for (int j = 0; j < matrix[0].Count; j += 2) {
        List<List<double>> z = new();

        z.Add(new List<double> {matrix[i][j], matrix[i][j+1]});
        z.Add(new List<double> {matrix[i+1][j], matrix[i+1][j+1]});

        matrices.Add(new(z));
      }
    }

    for (int i = 0; i < matrices.Count; i += 2) {
      MatrixData.Add(matrices[i..(i+2)]);
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

  public static BlockMatrix operator* (
      BlockMatrix A, BlockMatrix B) {
    List<List<Matrix>> result = new(); 

    for (int i = 0; i < A.MatrixData.Count; ++i) {
      List<Matrix> row = new();

      for (int j = 0; j < A.MatrixData[0].Count; ++j) {
        row.Add(A.GetRow(i).MultiplyLists(B.GetColumn(j)));
      }

      result.Add(row);
    }    

    return new(result);
  }

  public static List<Matrix> operator* (
      List<Matrix> row, BlockMatrix A) {
    List<Matrix> result = new();

    for (int i = 0; i < row.Count; ++i) {
      var column = A.GetColumn(i);

      result.Add(row.MultiplyLists(column));
    }

    return result;
  }

  public static BlockMatrix MultiplyWisely(
      BlockMatrix A, BlockMatrix B,
      List<(string, int)> ips) {
    BlockMatrix result = new(new List<List<double>>() {
      new() {2, 2, 3, 4},
      new() {4, 5, 6, 1},
      new() {2, 3, 4, 5},
      new() {5, 2, 1, 3},
    });

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
}
